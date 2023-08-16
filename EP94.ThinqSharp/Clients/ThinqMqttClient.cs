using EP94.ThinqSharp.Interfaces;
using EP94.ThinqSharp.Models;
using EP94.ThinqSharp.Models.Requests;
using EP94.ThinqSharp.Utils;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EP94.ThinqSharp.Clients
{
    internal delegate void StateChanged(ThinqMqttClientState previousState, ThinqMqttClientState newState);
    internal class ThinqMqttClient : IDisposable
    {
        public StateChanged? OnStateChanged;
        public ThinqMqttClientState State
        {
            get => _state;
            set
            {
                if (_state != value)
                {
                    ThinqMqttClientState previous = _state;
                    _state = value;
                    OnStateChanged?.Invoke(previous, value);
                }
            }
        }
        private ThinqMqttClientState _state = ThinqMqttClientState.NotConnected;

        private readonly Passport _passport;
        private readonly Route _route;
        private readonly Gateway _gateway;
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        private bool _disposed;
        private X509Certificate2? _certificate;
        private readonly Uri _brokerUri;
        private IMqttClient? _client;
        private IEnumerable<string>? _topics;
        private MqttClientOptions? _options;

        private readonly SemaphoreSlim _reconnectSemaphore;
        private volatile bool _disconnectRequested;
        private int _reconnectTimeout = 1;
        private readonly string _clientId;
        private readonly Dictionary<string, ISnapshot> _attachedSnapshots;
        private const int PingInterval = 30000;
        private SemaphoreSlim _pingSemaphore;

        public ThinqMqttClient(Passport passport, Route route, Gateway gateway, ILoggerFactory loggerFactory, string clientId)
        {
            _passport = passport;
            _route = route;
            _gateway = gateway;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<ThinqMqttClient>();
            _brokerUri = new Uri(route.MqttServer);
            _reconnectSemaphore = new SemaphoreSlim(1);
            _clientId = clientId;
            _attachedSnapshots = new Dictionary<string, ISnapshot>();
            _pingSemaphore = new SemaphoreSlim(1);
        }

        public async Task ConnectAsync()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ThinqMqttClient));
            string csr = X509CertificateHelpers.CreateCsr(out AsymmetricCipherKeyPair keyPair);
            RegisterIotCertificateRequest registerIotCertificateRequest = new RegisterIotCertificateRequest(_clientId, _passport, _gateway, _loggerFactory, csr);
            IotCertificate iotCertificate = await registerIotCertificateRequest.RegisterIotCertificateAsync();
            _topics = iotCertificate.Subscriptions;
            using X509Certificate2 certificate = iotCertificate.CertificatePemCertificate;
            _certificate = certificate.CopyWithPrivateKey(keyPair.Private);
            _options = CreateOptions(_certificate, _clientId, _brokerUri);
            _client = new MqttFactory().CreateMqttClient();
            _client.DisconnectedAsync += OnDisconnectAsync;
            _client.ConnectedAsync += OnConnectedAsync;
            _client.ApplicationMessageReceivedAsync += OnMessageReceivedAsync;
            _logger.LogInformation("Connecting to mqtt broker {BrokerUri}", _brokerUri);
            try
            {
                await _client.ConnectAsync(_options);
            }
            catch { }
        }

        public async Task DisconnectAsync()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ThinqMqttClient));
            _disconnectRequested = true;
            _logger.LogInformation("Disconnecting mqtt client");
            await _reconnectSemaphore.WaitAsync();
            await _client.DisconnectAsync();
            _reconnectSemaphore.Release();
            _logger.LogInformation("Disconnected mqtt client");
        }

        public void Attach(string deviceId, ISnapshot snapshot)
        {
            _attachedSnapshots[deviceId] = snapshot;
        }

        public void Detach(string deviceId)
        {
            _attachedSnapshots.Remove(deviceId);
        }

        private Task OnMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs args)
        {
            string payload = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
            _logger.LogDebug("Mqtt message received: {Message}", payload);
            JObject jObject = JObject.Parse(payload);
            if (jObject.TryGetValue(nameof(MqttPayload.Type).ToLower(), out JToken? value) && Equals(value.Value<string>(), "monitoring"))
            {
                MqttPayload? mqttPayload = jObject.ToObject<MqttPayload>();
                if (mqttPayload is null)
                {
                    _logger.LogError("Failed deserializing mqtt payload, message ignored");
                    return Task.CompletedTask;
                }
                if (_attachedSnapshots.TryGetValue(mqttPayload.DeviceId, out ISnapshot? snapshot))
                {
                    snapshot.Merge(mqttPayload.Data.State.Reported);
                }
            }
            else
            {
                _logger.LogDebug("Ignoring payload, not a monitoring message");
            }
           
            return Task.CompletedTask;
        }

        private async Task OnConnectedAsync(MqttClientConnectedEventArgs args)
        {
            State = ThinqMqttClientState.Connected;
            _reconnectTimeout = 1;
            _logger.LogInformation("Connected to mqtt broker {BrokerUri}", _brokerUri);
            _ = Task.Run(PingAsync);
            if (_topics is null || !_topics.Any())
            {
                _logger.LogError("No topics to subscribe to!");
            }
            foreach (string topic in _topics ?? Array.Empty<string>())
            {
                _logger.LogDebug("Subscribe to topic {Topic}", topic);
                await _client.SubscribeAsync(new MqttTopicFilter() { Topic = topic });
            }
        }

        private async Task PingAsync()
        {
            if (!await _pingSemaphore.WaitAsync(PingInterval))
            {
                _logger.LogDebug("Not starting a ping task, because one is already running");
                return;
            }
            try
            {
                while (_client is not null && _client.IsConnected)
                {
                    _logger.LogDebug("Sending ping");
                    await _client.PingAsync();
                    await Task.Delay(PingInterval);
                }
                _logger.LogDebug("Ping stopped gracefully");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ping stopped because of exception");
            }
            finally
            {
                _pingSemaphore.Release();
            }
        }

        private async Task OnDisconnectAsync(MqttClientDisconnectedEventArgs args)
        {
            if (_disposed) return;
            await _reconnectSemaphore.WaitAsync();
            switch (State)
            {
                case ThinqMqttClientState.NotConnected:
                    _logger.LogError("Initial connection failed to mqtt broker {BrokerUri}", _brokerUri);
                    break;

                case ThinqMqttClientState.Connected:
                    _logger.LogError("Disconnected from mqtt broker {BrokerUri}", _brokerUri);
                    break;
            }
            State = ThinqMqttClientState.Disconnected;
            if (_disconnectRequested)
            {
                _logger.LogDebug("Stopped reconnecting because disconnect is requested");
                _reconnectSemaphore.Release();
                return;
            }
            if (_client is null || _options is null)
            {
                _logger.LogError("Stopped reconnecting because the mqtt client became null");
                _reconnectSemaphore.Release();
                return;
            }
            try
            {
                _reconnectTimeout = Math.Min(_reconnectTimeout * 2, 16);
                await Task.Delay(TimeSpan.FromSeconds(_reconnectTimeout));
                await _client.ConnectAsync(_options);
            }
            catch { }
            _reconnectSemaphore.Release();
        }

        private static MqttClientOptions CreateOptions(X509Certificate2 certificate, string clientId, Uri brokerUri)
        {
            return new MqttClientOptions
            {
                ChannelOptions = new MqttClientTcpOptions
                {
                    Server = brokerUri.Host,
                    Port = brokerUri.Port,
                    TlsOptions = new MqttClientTlsOptions()
                    {
                        UseTls = true,
                        AllowUntrustedCertificates = true,
                        Certificates = new List<X509Certificate>() { certificate },
                        CertificateValidationHandler = (c) =>
                        {
                            return true;
                        },
                        SslProtocol = SslProtocols.None
                    }
                },
                ClientId = clientId
            };
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _reconnectSemaphore.Wait();
                    _certificate?.Dispose();
                    _client?.Dispose();
                    _reconnectSemaphore.Dispose();
                }
                _attachedSnapshots.Clear();
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposed = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ThinqMqttClient()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

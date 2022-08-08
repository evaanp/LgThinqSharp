using EP94.ThinqSharp.Clients;
using EP94.ThinqSharp.Models;
using EP94.ThinqSharp.Models.Requests;
using EP94.ThinqSharp.Utils;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Globalization;
using EP94.ThinqSharp.Exceptions;

namespace EP94.ThinqSharp
{
    public sealed class ThinqClient : IDisposable
    {
        private string? _username;
        private string? _password;
        private Passport? _passport;
        private ILoggerFactory _loggerFactory;
        private ILogger<ThinqClient> _logger;
        private Gateway? _gateway;
        private string _clientId;
        private ThinqMqttClient? _mqttClient;
        private List<DeviceClient>? _deviceClients;
        private bool _disposed;

        /// <summary>
        /// The constructor to provide the passport to the client, this will speed up the login process.
        /// </summary>
        /// <param name="passport"></param>
        /// <param name="loggerFactory"></param>
        public ThinqClient(Passport passport, ILoggerFactory? loggerFactory = null)
            : this(loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(passport, nameof(passport));
            _passport = passport;
            _clientId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// The default constructor, for when the passport object is not saved or not yet requested
        /// </summary>
        /// <param name="loggerFactory"></param>
        public ThinqClient(ILoggerFactory? loggerFactory = null)
        {
            _loggerFactory = loggerFactory ?? LoggerFactory.Create(_ => { });
            _logger = _loggerFactory.CreateLogger<ThinqClient>();
            _clientId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Get the passport needed to authenticate with the lg thinq api. When saved and provided via the dedicated constructor, the login process will be a lot faster.
        /// </summary>
        /// <param name="username">The username of the lg account</param>
        /// <param name="password">The password of the lg account</param>
        /// <param name="cultureInfo">The culture used by the lg account, default = current culture</param>
        /// <param name="silent">Should error messages be logged?</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException">There is something wrong with the http request, or the response code doesn't indicate success</exception>
        /// <exception cref="ThinqApiException">There is something wrong with the response the thinq api has provided</exception>
        public async Task<Passport> GetPassportAsync(string username, string password, CultureInfo? cultureInfo = null, bool silent = false)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ThinqClient));
            ArgumentNullException.ThrowIfNull(username, nameof(username));
            ArgumentNullException.ThrowIfNull(password, nameof(password));
            _logger.LogInformation("Start getting passport");
            cultureInfo ??= CultureInfo.CurrentCulture;
            GatewayRequest gatewayRequest = new GatewayRequest(_clientId, cultureInfo, _loggerFactory);
            _gateway = await gatewayRequest.GetGatewayAsync(silent);
            PreLoginRequest preLoginRequest = new PreLoginRequest(_loggerFactory, _gateway, username, password.EncodeSHA512());
            PreLoginResult preLoginResult = await preLoginRequest.GetPreLoginResult(silent);
            LoginRequest loginRequest = new LoginRequest(_loggerFactory, _gateway, preLoginResult, username);
            Account account = await loginRequest.Login(silent);
            GetSecretOAuthKeyRequest getSecretOAuthKeyRequest = new GetSecretOAuthKeyRequest(_gateway, _loggerFactory);
            string secretOAuthKey = await getSecretOAuthKeyRequest.GetSecretOAuthKey(silent);
            AuthorizeEmpRequest authorizeEmpRequest = new AuthorizeEmpRequest(_loggerFactory, account, secretOAuthKey);
            AuthorizeEmpResponse authorizeEmpResponse = await authorizeEmpRequest.AuthorizeEmp(silent);
            GetOAuthTokenRequest getOAuthTokenRequest = new GetOAuthTokenRequest(_loggerFactory, authorizeEmpResponse);
            OAuthToken oAuthToken = await getOAuthTokenRequest.GetOAuthToken(silent);
            UserProfileRequest userProfileRequest = new UserProfileRequest(oAuthToken.AccessToken, authorizeEmpResponse, _loggerFactory);
            UserProfile userProfile = await userProfileRequest.GetUserProfile(silent);
            _logger.LogInformation("Getting passport successful");
            _passport = new Passport(oAuthToken, userProfile, cultureInfo.TwoLetterISOLanguageName.ToUpper(), cultureInfo.Name);
            return _passport;
        }


        /// <summary>
        /// Connect the client to the thinq api
        /// </summary>
        /// <param name="silent">Should error messages be logged?</param>
        /// <returns>The supported device clients</returns>
        /// <exception cref="HttpRequestException">There is something wrong with the http request, or the response code doesn't indicate success</exception>
        /// <exception cref="ThinqApiException">There is something wrong with the response the thinq api has provided</exception>
        public async Task<IEnumerable<DeviceClient>> ConnectAsync(bool silent = false)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ThinqClient));
            if (_passport is null)
            {
                throw new InvalidOperationException($"Passport not provided, call {nameof(GetPassportAsync)} to retrieve one");
            }
            // Gateway is null when the passport constructor is used
            if (_gateway is null)
            {
                GatewayRequest gatewayRequest = new GatewayRequest(_clientId, _passport, _loggerFactory);
                _gateway = await gatewayRequest.GetGatewayAsync(silent);
            }
            RouteRequest routeRequest = new RouteRequest(_clientId, _passport, _loggerFactory);
            Route route = await routeRequest.GetRouteAsync(silent);
            _mqttClient = new ThinqMqttClient(_passport, route, _gateway, _loggerFactory, _clientId);
            await _mqttClient.ConnectAsync();
            _mqttClient.OnStateChanged += OnMqttClientStateChanged;
            GetDevicesInfoRequest getDevicesInfoRequest = new GetDevicesInfoRequest(_clientId, _passport, _gateway, _loggerFactory);
            IEnumerable<DeviceInfo> devices = await getDevicesInfoRequest.GetDevicesInfo(silent);

            List<DeviceClient> deviceClients = new List<DeviceClient>();
            foreach (DeviceInfo deviceInfo in devices)
            {
                DeviceClient? deviceClient = GetDeviceClient(deviceInfo, _passport, _gateway, _mqttClient);
                if (deviceClient is null)
                {
                    _logger.LogWarning("Device with type code {DeviceType} not yet supported, device ignored", deviceInfo.DeviceType);
                    continue;
                }
                deviceClients.Add(deviceClient);

                // Reloading snapshot as the snapshot could be changed between the initial load and the attachment to the mqtt client
                await deviceClient.ReloadSnapshotAsync();
            }
            _deviceClients = deviceClients;
            return deviceClients;
        }

        /// <summary>
        /// Disconnect the client from the thinq api
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public async Task DisconnectAsync()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ThinqClient));
            if (_mqttClient is not null)
            {
                _logger.LogInformation("Disconnecting...");
                if (_deviceClients is not null)
                {
                    foreach (DeviceClient deviceClient in _deviceClients)
                    {
                        _mqttClient.Detach(deviceClient.DeviceInfo.DeviceId);
                    }
                }
                await _mqttClient.DisconnectAsync();
                _logger.LogInformation("Disconnected");
            }
            else
            {
                _logger.LogInformation("Not disconnecting, not connected");
            }
        }

        private void OnMqttClientStateChanged(ThinqMqttClientState previousState, ThinqMqttClientState newState)
        {
            if (previousState == ThinqMqttClientState.Disconnected && newState == ThinqMqttClientState.Connected && _deviceClients is not null)
            {
                _logger.LogInformation("Reloading snapshots because the mqtt connection was restored");
                _ = Parallel.ForEachAsync(_deviceClients, async (deviceCient, cancellationToken) =>
                {
                    await deviceCient.ReloadSnapshotAsync();
                });
            }
        }


        private DeviceClient? GetDeviceClient(DeviceInfo deviceInfo, Passport passport, Gateway gateway, ThinqMqttClient mqttClient)
        {
            return deviceInfo.DeviceType switch
            {
                (int)DeviceType.AC => new AcClient(_clientId, _logger, _loggerFactory, deviceInfo, passport, gateway, mqttClient),
                _ => null
            };
        }

        public void Dispose()
        {
            if (_mqttClient is not null)
            {
                _mqttClient.OnStateChanged -= OnMqttClientStateChanged;
                _mqttClient.Dispose();
            }
        }
    }
}

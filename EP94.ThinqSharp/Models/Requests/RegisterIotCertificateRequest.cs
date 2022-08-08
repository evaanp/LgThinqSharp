using EP94.ThinqSharp.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models.Requests
{
    internal class RegisterIotCertificateRequest : ThinqApiRequest<IotCertificate>
    {
        private Passport _passport;
        private Gateway _gateway;
        private ILoggerFactory _loggerFactory;
        private string _clientId;

        public RegisterIotCertificateRequest(string clientId, Passport passport, Gateway gateway, ILoggerFactory loggerFactory, string csr)
            : base(clientId, passport, HttpMethod.Post, $"{gateway.Thinq2Uri}/service/users/client/certificate", RequestType.Json, loggerFactory.CreateLogger<RegisterIotCertificateRequest>(), loggerFactory, GetBody(csr))
        {
            _passport = passport;
            _gateway = gateway;
            _loggerFactory = loggerFactory;
            _clientId = clientId;
        }

        public async Task<IotCertificate> RegisterIotCertificateAsync()
        {
            Logger.LogDebug("Registering iot certificate");
            try
            {
                CheckDeviceRegistrationRequest checkDeviceRegistrationRequest = new CheckDeviceRegistrationRequest(_clientId, _passport, _gateway, _loggerFactory);
                bool deviceRegistered = await checkDeviceRegistrationRequest.CheckDeviceRegistrationAsync();
                if (!deviceRegistered)
                {
                    Logger.LogDebug("Device not yet registered, register device");
                    RegisterDeviceRequest registerDeviceRequest = new RegisterDeviceRequest(_clientId, _passport, _gateway, _loggerFactory);
                    await registerDeviceRequest.RegisterDeviceAsync();
                    Logger.LogDebug("Device registered");
                }
                IotCertificate iotCertificate = await ExecuteThinqApiRequestWithResponseAsync();
                Logger.LogDebug("Registering iot certificate successful");
                return iotCertificate;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Exception occured while registering iot certificate");
                throw;
            }
        }

        private static Dictionary<string, string> GetBody(string csr)
        {
            return new Dictionary<string, string>
            {
                { "csr", csr },
            };
        }
    }
}

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models.Requests
{
    internal class RegisterDeviceRequest : ThinqApiRequest<string>
    {
        public RegisterDeviceRequest(string clientId, Passport passport, Gateway gateway, ILoggerFactory loggerFactory)
            : base(clientId, passport, HttpMethod.Post, $"{gateway.Thinq2Uri}/service/users/client", RequestType.Json, loggerFactory.CreateLogger<RegisterDeviceRequest>(), loggerFactory)
        {

        }

        public async Task RegisterDeviceAsync()
        {
            Logger.LogDebug("Registering device");
            try
            {
                await ExecuteThinqApiRequestWithResponseAsync();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Exception occured while registering device");
                throw;
            }
        }
    }
}

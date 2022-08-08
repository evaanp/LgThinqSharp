using EP94.ThinqSharp.Config;
using EP94.ThinqSharp.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models.Requests
{
    internal class CheckDeviceRegistrationRequest : ThinqApiRequest<string>
    {
        public CheckDeviceRegistrationRequest(string clientId, Passport passport, Gateway gateway, ILoggerFactory loggerFactory)
            : base(clientId, passport, HttpMethod.Get, $"{gateway.Thinq2Uri}/service/users/client", RequestType.Json, loggerFactory.CreateLogger<CheckDeviceRegistrationRequest>(), loggerFactory)
        {

        }

        public async Task<bool> CheckDeviceRegistrationAsync()
        {
            Logger.LogDebug("Checking device registration");
            try
            {
                await ExecuteThinqApiRequestWithResponseAsync();
                Logger.LogDebug("Device already registered");
                return true;
            }
            catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.BadRequest)
            {
                Logger.LogDebug("Device is not yet registered");
                return false;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Exception occured while checking device registration");
                throw;
            }
        }
    }
}

using EP94.ThinqSharp.Exceptions;
using Microsoft.Extensions.Logging;

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
            catch (ThinqApiException e) when (e.ErrorCode == ThinqResponseCode.NOT_EXIST_DATA)
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

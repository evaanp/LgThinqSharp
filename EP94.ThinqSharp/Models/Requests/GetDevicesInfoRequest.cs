using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EP94.ThinqSharp.Exceptions;

namespace EP94.ThinqSharp.Models.Requests
{
    internal class GetDevicesInfoRequest : ThinqApiRequest<DevicesInfoResponse>
    {
        public GetDevicesInfoRequest(string clientId, Passport passport, Gateway gateway, ILoggerFactory loggerFactory)
            : base (clientId, passport, HttpMethod.Get, $"{gateway.Thinq2Uri}/service/application/dashboard", RequestType.UrlEncoded, loggerFactory.CreateLogger<GetDevicesInfoRequest>(), loggerFactory)
        {
        }

        /// <summary>
        /// Get the devices info
        /// </summary>
        /// <param name="silent">Don't log possible errors</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="ThinqApiException"></exception>
        public async Task<IEnumerable<DeviceInfo>> GetDevicesInfo(bool silent)
        {
            Logger.LogDebug("Executing get devices info request");
            try
            {
                DevicesInfoResponse response = await ExecuteThinqApiRequestWithResponseAsync();
                Logger.LogDebug("Get devices info request succeeded");
                return response.DevicesInfo;
            }
            catch (Exception e)
            {
                if (!silent)
                {
                    Logger.LogError(e, "Exception occured while requesting devices info");
                }
                throw;
            }
        }
    }
}

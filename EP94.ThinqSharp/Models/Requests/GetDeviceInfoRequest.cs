using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models.Requests
{
    internal class GetDeviceInfoRequest : ThinqApiRequest<DeviceInfo>
    {
        private readonly string _deviceId;
        public GetDeviceInfoRequest(string clientId, Passport passport, Gateway gateway, string deviceId, ILoggerFactory loggerFactory)
            : base(clientId, passport, HttpMethod.Get, $"{gateway.Thinq2Uri}/service/devices/{deviceId}", RequestType.UrlEncoded, loggerFactory.CreateLogger<GetDeviceInfoRequest>(), loggerFactory)
        {
            _deviceId = deviceId;
        }

        public async Task<DeviceInfo> GetDeviceInfoAsync(bool silent)
        {
            Logger.LogDebug("Getting device info for device {Device}", _deviceId);
            try
            {
                DeviceInfo deviceInfo = await ExecuteThinqApiRequestWithResponseAsync();
                Logger.LogDebug("Getting device info for device {Device} succeeded", _deviceId);
                return deviceInfo;
            }
            catch (Exception e)
            {
                if (!silent)
                {
                    Logger.LogError(e, "Exception occured while getting device info for device {Device}", _deviceId);
                }
                throw;
            }
        }
    }
}

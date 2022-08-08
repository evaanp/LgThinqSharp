using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models
{
    public class DevicesInfoResponse
    {
        public string LangPackCommonVer { get; set; }
        public string LangPackCommonUri { get; set; }
        [JsonProperty("item")]
        public List<DeviceInfo> DevicesInfo { get; set; }

        public DevicesInfoResponse(string langPackCommonVer, string langPackCommonUri, List<DeviceInfo> devicesInfo)
        {
            LangPackCommonVer = langPackCommonVer;
            LangPackCommonUri = langPackCommonUri;
            DevicesInfo = devicesInfo;
        }
    }
}

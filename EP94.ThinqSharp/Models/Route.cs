using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models
{
    internal class Route
    {
        public string ApiServer { get; set; }
        public string MqttServer { get; set; }

        public Route(string apiServer, string mqttServer)
        {
            ApiServer = apiServer;
            MqttServer = mqttServer;
        }
    }
}

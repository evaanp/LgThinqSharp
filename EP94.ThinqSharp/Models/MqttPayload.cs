using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models
{
    internal class MqttPayload
    {
        public Data Data { get; set; }
        public string DeviceId { get; set; }
        public string Type { get; set; }

        public MqttPayload(Data data, string deviceId, string type)
        {
            Data = data;
            DeviceId = deviceId;
            Type = type;
        }
    }

    internal class Data
    {
        public State State { get; set; }

        public Data(State state)
        {
            State = state;
        }
    }

    internal class State
    {
        public Dictionary<string, object> Reported { get; set; }

        public State(Dictionary<string, object> reported)
        {
            Reported = reported;
        }
    }
}

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EP94.ThinqSharp.Exceptions;

namespace EP94.ThinqSharp.Models.Requests
{
    internal class CommandRequest : ThinqApiRequest<object>
    {
        private string _command;
        private string _dataKey;
        private object _value;

        public CommandRequest(string clientId, Passport passport, Gateway gateway, DeviceInfo deviceInfo, ILoggerFactory loggerFactory, string command, string dataKey, object value)
            : base(clientId, passport, HttpMethod.Post, $"{gateway.Thinq2Uri}/service/devices/{deviceInfo.DeviceId}/control-sync", RequestType.Json, loggerFactory.CreateLogger<CommandRequest>(), loggerFactory, GetBody(command, dataKey, value))
        {
            _command = command;
            _dataKey = dataKey;
            _value = value;
        }

        /// <summary>
        /// Send a command to the device
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="ThinqApiException"></exception>
        public async Task ExecuteCommand()
        {
            Logger.LogDebug("Executing command {Command} with data key {DataKey} with value {Value}", _command, _dataKey, _value);
            try
            {
                await ExecuteThinqApiRequestWithResponseAsync();
                Logger.LogDebug("Executing command {Command} with data key {DataKey} with value {Value} succeeded", _command, _dataKey, _value);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Executing command {Command} with data key {DataKey} with value {Value} failed", _command, _dataKey, _value);
                throw;
            }
        }

        private static Dictionary<string, string> GetBody(string command, string dataKey, object value)
        {
            return new Dictionary<string, string>
            {
                { "command", command },
                { "dataKey", dataKey },
                { "dataValue", ValueToString(value) },
                { "ctrlKey", "basicCtrl" }
            };
        }

        private static string ValueToString(object value)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            return value switch
            {
                int intValue => intValue.ToString(),
                float floatValue => floatValue.ToString(nfi),
                bool boolValue => boolValue ? "1" : "0",
                _ => value.ToString() ?? string.Empty
            };
        }
    }
}

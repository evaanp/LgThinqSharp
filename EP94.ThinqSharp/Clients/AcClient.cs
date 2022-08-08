using EP94.ThinqSharp.Models;
using EP94.ThinqSharp.Models.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EP94.ThinqSharp.Exceptions;
using System.Linq.Expressions;

namespace EP94.ThinqSharp.Clients
{
    public class AcClient : DeviceClient
    {
        internal AcClient(string clientId, ILogger logger, ILoggerFactory loggerFactory, DeviceInfo deviceInfo, Passport passport, Gateway gateway, ThinqMqttClient thinqMqttClient) : base(clientId, logger, loggerFactory, deviceInfo, passport, gateway, thinqMqttClient, new AcSnapshot(deviceInfo.Snapshot))
        {

        }

        public Task SetSnapshotValue<T>(Expression<Func<AcSnapshot, T>> propertyLambda, T value) => base.SetSnapshotValue<AcSnapshot, T>(propertyLambda, value);

        /// <summary>
        /// Turn the ac on or off
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="ThinqApiException"></exception>
        public Task SetOnState(bool on) => SetSnapshotValue(s => s.IsOn, on);
        /// <summary>
        /// Set the vertical fan direction
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="ThinqApiException"></exception>
        public Task SetVerticalStep(int step) => SetSnapshotValue(s => s.VerticalStep, step);
        /// <summary>
        /// Set the horizontal fan direction
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="ThinqApiException"></exception>
        public Task SetHorizontalStep(int step) => SetSnapshotValue(s => s.HorizontalStep, step);
        /// <summary>
        /// Set the ac mode
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="ThinqApiException"></exception>
        public Task SetMode(AcMode mode) => SetSnapshotValue(s => s.OperationMode, (int)mode);
        /// <summary>
        /// Set the temperature setpoint
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="ThinqApiException"></exception>
        public Task SetTemperatureSetpoint(int setpoint) => SetSnapshotValue(s => s.TargetTemperature, setpoint);
        /// <summary>
        /// Set the fan speed
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="ThinqApiException"></exception>
        public Task SetFanSpeed(AcFanSpeed speed) => SetSnapshotValue(s => s.FanSpeed , (int)speed);
    }
}

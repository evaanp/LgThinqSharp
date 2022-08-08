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
using System.Reflection;
using Newtonsoft.Json;
using EP94.ThinqSharp.Interfaces;

namespace EP94.ThinqSharp.Clients
{
    public abstract class DeviceClient
    {
        public EventHandler? OnDeviceSnapshotChanged;
        public DeviceInfo DeviceInfo { get; }
        public ISnapshot DeviceSnapshot { get; }
        protected ILogger Logger { get; }
        protected Passport Passport { get; }
        protected ILoggerFactory LoggerFactory { get; }
        private Gateway _gateway;
        private string _clientId;

        internal DeviceClient(string clientId, ILogger logger, ILoggerFactory loggerFactory, DeviceInfo deviceInfo, Passport passport, Gateway gateway, ThinqMqttClient thinqMqttClient, ISnapshot deviceSnapshot)
        {
            Logger = logger;
            LoggerFactory = loggerFactory;
            DeviceInfo = deviceInfo;
            Passport = passport;
            _gateway = gateway;
            _clientId = clientId;
            DeviceSnapshot = deviceSnapshot;
            thinqMqttClient.Attach(DeviceInfo.DeviceId, DeviceSnapshot);
            DeviceSnapshot.ValueChanged += SnapshotChanged;
        }

        private void SnapshotChanged(object? sender, EventArgs e)
        {
            OnDeviceSnapshotChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task ReloadSnapshotAsync()
        {
            GetDeviceInfoRequest getDeviceInfoRequest = new GetDeviceInfoRequest(_clientId, Passport, _gateway, DeviceInfo.DeviceId, LoggerFactory);
            DeviceInfo deviceInfo = await getDeviceInfoRequest.GetDeviceInfoAsync(false);
            DeviceSnapshot.Merge(deviceInfo.Snapshot);
        }

        protected async Task SetSnapshotValue<TSnapshot, T>(Expression<Func<TSnapshot, T>> propertyLambda, T value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            PropertyInfo propertyInfo = GetPropertyInfo(propertyLambda);
            JsonPropertyAttribute? jsonPropertyAttribute = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>();
            string name = jsonPropertyAttribute?.PropertyName ?? propertyInfo.Name;
            await SendCommand(name.Contains("operation") ? "Operation" : "Set", name, value);
        }

        private PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof(TSource);

            MemberExpression? member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            PropertyInfo? propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            if (type != propInfo.ReflectedType)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));

            return propInfo;
        }

        /// <summary>
        /// Send a command to the device
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="ThinqApiException"></exception>
        protected Task SendCommand(string command, string dataKey, object value)
        {
            CommandRequest commandRequest = new CommandRequest(_clientId, Passport, _gateway, DeviceInfo, LoggerFactory, command, dataKey, value);
            return commandRequest.ExecuteCommand();
        }
    }
}

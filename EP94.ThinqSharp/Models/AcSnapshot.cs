using EP94.ThinqSharp.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models
{
    public class AcSnapshot : ISnapshot
    {
        public event EventHandler? ValueChanged;
        [JsonIgnore]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        [JsonProperty("airState.windStrength")]
        public double? FanSpeed { get; init; }
        [JsonProperty("airState.wMode.lowHeating")]
        public double? ModeLowHeating { get; init; }
        [JsonProperty("airState.diagCode")]
        public double? DiagCode { get; init; }
        [JsonProperty("airState.lightingState.displayControl")]
        public double? LightingStateDisplayControl { get; init; }
        [JsonProperty("airState.wDir.hStep")]
        public double? HorizontalStep { get; init; }
        [JsonProperty("mid")]
        public double? Mid { get; init; }
        [JsonProperty("airState.energy.onCurrent")]
        public double? CurrentEnergy { get; init; }
        [JsonProperty("airState.wMode.airClean")]
        public bool? ModeAirClean { get; init; }
        [JsonProperty("airState.quality.sensorMon")]
        public double? SensorMon { get; init; }
        [JsonProperty("airState.tempState.target")]
        public double? TargetTemperature { get; init; }
        [JsonProperty("airState.operation")]
        public bool? IsOn { get; init; }
        [JsonProperty("airState.wMode.jet")]
        public bool? JetMode { get; init; }
        [JsonProperty("airState.wDir.vStep")]
        public double? VerticalStep { get; init; }
        [JsonProperty("timestamp")]
        public double? Timestamp { get; init; }
        [JsonProperty("airState.powerSave.basic")]
        public double? PowersafeBasic { get; init; }
        public Static? Static { get; init; }
        [JsonProperty("airState.tempState.current")]
        public double? MeasuredTemperature { get; init; }
        [JsonProperty("airState.miscFuncState.extraOp")]
        public double? ExtraOp { get; init; }
        [JsonProperty("airState.reservation.sleepTime")]
        public double? SleepTime { get; init; }
        [JsonProperty("airState.miscFuncState.autoDry")]
        public bool? AutoDry { get; init; }
        [JsonProperty("airState.reservation.targetTimeToStart")]
        public double? TargetTimeToStart { get; init; }
        public Meta? Meta { get; init; }
        public bool? Online { get; init; }
        [JsonProperty("airState.opMode")]
        public double? OperationMode { get; init; }
        [JsonProperty("airState.reservation.targetTimeToStop")]
        public double? TargetTimeToStop { get; init; }
        [JsonProperty("airState.filterMngStates.maxTime")]
        public double? FilterMaxTime { get; init; }
        [JsonProperty("airState.filterMngStates.useTime")]
        public double? FilterUseTime { get; init; }
        [JsonProperty("airState.tempState.limitMin")]
        public double? LimitMin { get; init; }
        [JsonProperty("airState.aroma.state")]
        public double? AromaState { get; init; }
        [JsonProperty("airState.filterMngStates.maxTimeBottom")]
        public double? MaxTimeBottom { get; init; }
        [JsonProperty("airState.miscFuncState.autoDryRemainTime")]
        public double? AutoDryRemainTime { get; init; }
        [JsonProperty("airState.tempState.airTempCoolMin")]
        public double? TempCoolMin { get; init; }
        [JsonProperty("airState.quality.odor")]
        public double? OdorQuality { get; init; }
        [JsonProperty("airState.miscFuncState.filterCleanAuto")]
        public double? FilterCleanAuto { get; init; }
        [JsonProperty("airState.homeCare.onOff")]
        public bool? HomeCareOnOff { get; init; }

        public AcSnapshot(Dictionary<string, object> initialValues)
        {
            Merge(initialValues);
        }

        //public void Merge(AcSnapshot other)
        //{
        //    bool valueHasChanged = false;
        //    foreach (PropertyInfo propertyInfo in GetType().GetProperties())
        //    {
        //        object? otherValue = propertyInfo.GetValue(other);
        //        object? currentValue = propertyInfo.GetValue(this);
        //        if (otherValue is not null && !Equals(otherValue, currentValue))
        //        {
        //            propertyInfo.SetValue(this, otherValue);
        //            valueHasChanged = true;
        //        }
        //    }
        //    if (valueHasChanged)
        //    {
        //        LastUpdated = DateTime.UtcNow;
        //        ValueChanged?.Invoke(this, EventArgs.Empty);
        //    }
        //}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void Merge(Dictionary<string, object> snapshot)
        {
            PropertyInfo[] properties = GetType().GetProperties();
            bool valueChanged = false;
            foreach (KeyValuePair<string, object> kvp in snapshot)
            {
                PropertyInfo? propertyInfo = properties.FirstOrDefault(p => kvp.Key.Equals(p.GetCustomAttribute<JsonPropertyAttribute>() is JsonPropertyAttribute jpa ? jpa.PropertyName : p.Name, StringComparison.OrdinalIgnoreCase));
                if (propertyInfo is null)
                {
                    continue;
                }
                if (kvp.Value is JObject jObject)
                {
                    propertyInfo.SetValue(this, jObject.ToObject(propertyInfo.PropertyType));
                }
                else
                {
                    object? currentValue = propertyInfo.GetValue(this);
                    if (!Equals(currentValue, kvp.Value))
                    {
                        valueChanged = true;
                        Type conversionType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                        propertyInfo.SetValue(this, Convert.ChangeType(kvp.Value, conversionType));
                    }
                }
            }
            if (valueChanged)
            {
                LastUpdated = DateTime.UtcNow;
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

namespace EP94.ThinqSharp.Models
{
    public class DeviceInfo
    {
        public string AppType { get; set; }
        public string ModelCountryCode { get; set; }
        public string CountryCode { get; set; }
        public string ModelName { get; set; }
        public int DeviceType { get; set; }
        public string DeviceCode { get; set; }
        public string Alias { get; set; }
        public string DeviceId { get; set; }
        public string FwVer { get; set; }
        public string ImageFileName { get; set; }
        public string ImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
        public string Ssid { get; set; }
        public string SoftapId { get; set; }
        public string SoftapPass { get; set; }
        public string MacAddress { get; set; }
        public string NetworkType { get; set; }
        public string TimezoneCode { get; set; }
        public string TimezoneCodeAlias { get; set; }
        public int? UtcOffset { get; set; }
        public string UtcOffsetDisplay { get; set; }
        public int? DstOffset { get; set; }
        public string DstOffsetDisplay { get; set; }
        public int? CurOffset { get; set; }
        public string CurOffsetDisplay { get; set; }
        public string SdsGuide { get; set; }
        public string NewRegYn { get; set; }
        public string RemoteControlType { get; set; }
        public string UserNo { get; set; }
        public string TftYn { get; set; }
        public float? ModelJsonVer { get; set; }
        public string ModelJsonUri { get; set; }
        public float? AppModuleVer { get; set; }
        public string AppModuleUri { get; set; }
        public string AppRestartYn { get; set; }
        public int? AppModuleSize { get; set; }
        public float? LangPackProductTypeVer { get; set; }
        public string LangPackProductTypeUri { get; set; }
        public string DeviceState { get; set; }
        public Dictionary<string, object> Snapshot { get; set; }
        public bool Online { get; set; }
        public string PlatformType { get; set; }
        public int? Area { get; set; }
        public float? RegDt { get; set; }
        public string BlackboxYn { get; set; }
        public string ModelProtocol { get; set; }
        public int? Order { get; set; }
        public string DrServiceYn { get; set; }
        public Fwinfolist[] FwInfoList { get; set; }
        public Modeminfo ModemInfo { get; set; }
        public string GuideTypeYn { get; set; }
        public string GuideType { get; set; }
        public string RegDtUtc { get; set; }
        public int? RegIndex { get; set; }
        public string GroupableYn { get; set; }
        public string ControllableYn { get; set; }
        public string CombinedProductYn { get; set; }
        public string MasterYn { get; set; }
        public string PccModelYn { get; set; }
        public Sdspid SdsPid { get; set; }
        public string AutoOrderYn { get; set; }
        public bool InitDevice { get; set; }
        public string ExistsEntryPopup { get; set; }
        public int? Tclcount { get; set; }

        public DeviceInfo(string appType, string modelCountryCode, string countryCode, string modelName, int deviceType, string deviceCode, string alias, string deviceId, string fwVer, string imageFileName, string imageUrl, string smallImageUrl, string ssid, string softapId, string softapPass, string macAddress, string networkType, string timezoneCode, string timezoneCodeAlias, int? utcOffset, string utcOffsetDisplay, int? dstOffset, string dstOffsetDisplay, int? curOffset, string curOffsetDisplay, string sdsGuide, string newRegYn, string remoteControlType, string userNo, string tftYn, float? modelJsonVer, string modelJsonUri, float? appModuleVer, string appModuleUri, string appRestartYn, int? appModuleSize, float? langPackProductTypeVer, string langPackProductTypeUri, string deviceState, Dictionary<string, object> snapshot, bool online, string platformType, int? area, float? regDt, string blackboxYn, string modelProtocol, int? order, string drServiceYn, Fwinfolist[] fwInfoList, Modeminfo modemInfo, string guideTypeYn, string guideType, string regDtUtc, int? regIndex, string groupableYn, string controllableYn, string combinedProductYn, string masterYn, string pccModelYn, Sdspid sdsPid, string autoOrderYn, bool initDevice, string existsEntryPopup, int? tclcount)
        {
            AppType = appType;
            ModelCountryCode = modelCountryCode;
            CountryCode = countryCode;
            ModelName = modelName;
            DeviceType = deviceType;
            DeviceCode = deviceCode;
            Alias = alias;
            DeviceId = deviceId;
            FwVer = fwVer;
            ImageFileName = imageFileName;
            ImageUrl = imageUrl;
            SmallImageUrl = smallImageUrl;
            Ssid = ssid;
            SoftapId = softapId;
            SoftapPass = softapPass;
            MacAddress = macAddress;
            NetworkType = networkType;
            TimezoneCode = timezoneCode;
            TimezoneCodeAlias = timezoneCodeAlias;
            UtcOffset = utcOffset;
            UtcOffsetDisplay = utcOffsetDisplay;
            DstOffset = dstOffset;
            DstOffsetDisplay = dstOffsetDisplay;
            CurOffset = curOffset;
            CurOffsetDisplay = curOffsetDisplay;
            SdsGuide = sdsGuide;
            NewRegYn = newRegYn;
            RemoteControlType = remoteControlType;
            UserNo = userNo;
            TftYn = tftYn;
            ModelJsonVer = modelJsonVer;
            ModelJsonUri = modelJsonUri;
            AppModuleVer = appModuleVer;
            AppModuleUri = appModuleUri;
            AppRestartYn = appRestartYn;
            AppModuleSize = appModuleSize;
            LangPackProductTypeVer = langPackProductTypeVer;
            LangPackProductTypeUri = langPackProductTypeUri;
            DeviceState = deviceState;
            Snapshot = snapshot;
            Online = online;
            PlatformType = platformType;
            Area = area;
            RegDt = regDt;
            BlackboxYn = blackboxYn;
            ModelProtocol = modelProtocol;
            Order = order;
            DrServiceYn = drServiceYn;
            FwInfoList = fwInfoList;
            ModemInfo = modemInfo;
            GuideTypeYn = guideTypeYn;
            GuideType = guideType;
            RegDtUtc = regDtUtc;
            RegIndex = regIndex;
            GroupableYn = groupableYn;
            ControllableYn = controllableYn;
            CombinedProductYn = combinedProductYn;
            MasterYn = masterYn;
            PccModelYn = pccModelYn;
            SdsPid = sdsPid;
            AutoOrderYn = autoOrderYn;
            InitDevice = initDevice;
            ExistsEntryPopup = existsEntryPopup;
            Tclcount = tclcount;
        }

        public override string ToString()
        {
            return DeviceId;
        }
    }

    public class Static
    {
        public string DeviceType { get; set; }
        public string CountryCode { get; set; }

        public Static(string deviceType, string countryCode)
        {
            DeviceType = deviceType;
            CountryCode = countryCode;
        }
    }

    public class Meta
    {
        public bool AllDeviceInfoUpdate { get; set; }
        public string MessageId { get; set; }

        public Meta(bool allDeviceInfoUpdate, string messageId)
        {
            AllDeviceInfoUpdate = allDeviceInfoUpdate;
            MessageId = messageId;
        }
    }

    public class Modeminfo
    {
        public string ModelName { get; set; }
        public string AppVersion { get; set; }
        public string ModemType { get; set; }
        public string RuleEngine { get; set; }

        public Modeminfo(string modelName, string appVersion, string modemType, string ruleEngine)
        {
            ModelName = modelName;
            AppVersion = appVersion;
            ModemType = modemType;
            RuleEngine = ruleEngine;
        }
    }

    public class Sdspid
    {
        public string Sds4 { get; set; }
        public string Sds3 { get; set; }
        public string Sds2 { get; set; }
        public string Sds1 { get; set; }

        public Sdspid(string sds4, string sds3, string sds2, string sds1)
        {
            Sds4 = sds4;
            Sds3 = sds3;
            Sds2 = sds2;
            Sds1 = sds1;
        }
    }

    public class Fwinfolist
    {
        public string Checksum { get; set; }
        public string PartNumber { get; set; }
        public float? Order { get; set; }

        public Fwinfolist(string checksum, string partNumber, float? order)
        {
            Checksum = checksum;
            PartNumber = partNumber;
            Order = order;
        }
    }
}

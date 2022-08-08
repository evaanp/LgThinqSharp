using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models
{
    internal class Account
    {
        [JsonProperty("loginSessionID")]
        public string LoginSessionID { get; set; }

        [JsonProperty("userID")]
        public string UserID { get; set; }

        [JsonProperty("userIDType")]
        public string UserIDType { get; set; }

        [JsonProperty("dateOfBirth")]
        public string DateOfBirth { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("countryName")]
        public string CountryName { get; set; }

        [JsonProperty("blacklist")]
        public string Blacklist { get; set; }

        [JsonProperty("age")]
        public string Age { get; set; }

        [JsonProperty("isSubscribe")]
        public string IsSubscribe { get; set; }

        [JsonProperty("isReceiveSms")]
        public string IsReceiveSms { get; set; }

        [JsonProperty("changePw")]
        public string ChangePw { get; set; }

        [JsonProperty("toEmailId")]
        public string ToEmailId { get; set; }

        [JsonProperty("periodPW")]
        public string PeriodPW { get; set; }

        [JsonProperty("lgAccount")]
        public string LgAccount { get; set; }

        [JsonProperty("isService")]
        public string IsService { get; set; }

        [JsonProperty("userNickName")]
        public string UserNickName { get; set; }

        [JsonProperty("termsList")]
        public List<object> TermsList { get; set; }

        [JsonProperty("userIDList")]
        public List<UserIDList> UserIDList { get; set; }

        [JsonProperty("serviceList")]
        public List<ServiceList> ServiceList { get; set; }

        [JsonProperty("displayUserID")]
        public string DisplayUserID { get; set; }

        [JsonProperty("authUser")]
        public string AuthUser { get; set; }

        [JsonProperty("dummyIdFlag")]
        public string DummyIdFlag { get; set; }

        public Account(string loginSessionID, string userID, string userIDType, string dateOfBirth, string country, string countryName, string blacklist, string age, string isSubscribe, string isReceiveSms, string changePw, string toEmailId, string periodPW, string lgAccount, string isService, string userNickName, List<object> termsList, List<UserIDList> userIDList, List<ServiceList> serviceList, string displayUserID, string authUser, string dummyIdFlag)
        {
            LoginSessionID = loginSessionID;
            UserID = userID;
            UserIDType = userIDType;
            DateOfBirth = dateOfBirth;
            Country = country;
            CountryName = countryName;
            Blacklist = blacklist;
            Age = age;
            IsSubscribe = isSubscribe;
            IsReceiveSms = isReceiveSms;
            ChangePw = changePw;
            ToEmailId = toEmailId;
            PeriodPW = periodPW;
            LgAccount = lgAccount;
            IsService = isService;
            UserNickName = userNickName;
            TermsList = termsList;
            UserIDList = userIDList;
            ServiceList = serviceList;
            DisplayUserID = displayUserID;
            AuthUser = authUser;
            DummyIdFlag = dummyIdFlag;
        }
    }

    internal class LgeIDList
    {
        [JsonProperty("lgeIDType")]
        public string LgeIDType { get; set; }

        [JsonProperty("userID")]
        public string UserID { get; set; }

        public LgeIDList(string lgeIDType, string userID)
        {
            LgeIDType = lgeIDType;
            UserID = userID;
        }
    }


    internal class ServiceList
    {
        [JsonProperty("svcCode")]
        public string SvcCode { get; set; }

        [JsonProperty("svcName")]
        public string SvcName { get; set; }

        [JsonProperty("isService")]
        public string IsService { get; set; }

        [JsonProperty("joinDate")]
        public string JoinDate { get; set; }

        public ServiceList(string svcCode, string svcName, string isService, string joinDate)
        {
            SvcCode = svcCode;
            SvcName = svcName;
            IsService = isService;
            JoinDate = joinDate;
        }
    }

    internal class UserIDList
    {
        [JsonProperty("lgeIDList")]
        public List<LgeIDList> LgeIDList { get; set; }

        public UserIDList(List<LgeIDList> lgeIDList)
        {
            LgeIDList = lgeIDList;
        }
    }


}

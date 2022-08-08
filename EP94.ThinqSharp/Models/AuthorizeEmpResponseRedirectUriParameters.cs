using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models
{
    internal class AuthorizeEmpResponseRedirectUriParameters
    {
        public string Code { get; set; }
        public string State { get; set; }
        [JsonProperty("user_number")]
        public string UserNumber { get; set; }
        [JsonProperty("oauth2_backend_url")]
        public string OAuth2BackendUrl { get; set; }

        public AuthorizeEmpResponseRedirectUriParameters(string code, string state, string userNumber, string oAuth2BackendUrl)
        {
            Code = code;
            State = state;
            UserNumber = userNumber;
            OAuth2BackendUrl = oAuth2BackendUrl;
        }
    }
}

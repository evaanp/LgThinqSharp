using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models
{
    internal class AuthorizeEmpResponse
    {
        public string Message { get; set; }

        [JsonProperty("redirect_uri")]
        public Uri RedirectUri { get; set; }

        public int Status { get; set; }

        [JsonIgnore]
        public AuthorizeEmpResponseRedirectUriParameters? RedirectUriParameters { get; set; }

        public AuthorizeEmpResponse(string message, Uri redirectUri, int status)
        {
            Message = message;
            RedirectUri = redirectUri;
            Status = status;
        }
    }
}

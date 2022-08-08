using EP94.ThinqSharp.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models.Requests
{
    internal class OAuthTokenRequest : OAuthRequest<OAuthToken>
    {
        private const string _relativeUrl = "/oauth/1.0/oauth2/token";
        public OAuthTokenRequest(ILogger logger, string backendUrl, Dictionary<string, string> queryParams)
            : base(HttpMethod.Post, logger, $"{backendUrl}oauth/1.0/oauth2/token", queryParams)
        {
        }

        public async Task<OAuthToken?> DoGetOAuthToken()
        {
            return await ExecuteRequestWithResponseAsync();
        }
    }
}

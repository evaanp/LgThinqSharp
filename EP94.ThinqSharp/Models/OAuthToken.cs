using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EP94.ThinqSharp
{
    public class OAuthToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("oauth2_backend_url")]
        public string OAuth2BackendUrl { get; set; }

        [JsonIgnore]
        public DateTime LastRefresh { get; set; } = DateTime.MinValue;

        public bool NeedsRefresh()
        {
            if (int.TryParse(ExpiresIn, out int expiresIn))
            {
                return DateTime.UtcNow - LastRefresh > TimeSpan.FromMinutes(expiresIn);
            }
            return true;
        }

        public void Update(OAuthToken other)
        {
            ExpiresIn = other.ExpiresIn;
            AccessToken = other.AccessToken;
            LastRefresh = DateTime.UtcNow;
        }

        public OAuthToken(string accessToken, string expiresIn, string refreshToken, string oAuth2BackendUrl)
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
            RefreshToken = refreshToken;
            OAuth2BackendUrl = oAuth2BackendUrl;
        }
    }
}

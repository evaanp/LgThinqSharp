using EP94.ThinqSharp.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models.Requests
{
    internal class RefreshOAuthTokenRequest : OAuthTokenRequest
    {
        private OAuthToken _token;
        public RefreshOAuthTokenRequest(Passport passport, ILoggerFactory loggerFactory)
            : base (loggerFactory.CreateLogger<RefreshOAuthTokenRequest>(), passport.Token.OAuth2BackendUrl, GetQueryParams(passport))
        {
            _token = passport.Token;
        }

        /// <summary>
        /// Refresh the oauth token
        /// </summary>
        /// <param name="silent">Don't log possible errors</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="ThinqApiException"></exception>
        public async Task RefreshOAuthToken(bool silent)
        {
            Logger.LogDebug("Executing oauth token request");
            try
            {
                OAuthToken? oAuthToken = await DoGetOAuthToken();
                if (oAuthToken is null)
                {
                    throw new ThinqApiException("Failed refreshing oauth token");
                }
                _token.Update(oAuthToken);
            }
            catch (Exception e)
            {
                if (!silent)
                {
                    Logger.LogError(e, "Exception occured while refreshing oauth token");
                }
                throw;
            }
        }

        private static Dictionary<string, string> GetQueryParams(Passport passport)
        {
            return new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", passport.Token.RefreshToken }
            };
        }
    }
}

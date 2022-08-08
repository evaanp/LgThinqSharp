using EP94.ThinqSharp.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models.Requests
{
    internal class GetOAuthTokenRequest : OAuthTokenRequest
    {
        public GetOAuthTokenRequest(ILoggerFactory loggerFactory, AuthorizeEmpResponse authorizeEmpResponse)
            : base (loggerFactory.CreateLogger<GetOAuthTokenRequest>(), GetBackendUrl(authorizeEmpResponse), GetQueryParams(authorizeEmpResponse))
        {

        }

        public async Task<OAuthToken> GetOAuthToken(bool silent)
        {
            Logger.LogDebug("Executing oauth token request");
            try
            {
                OAuthToken? oAuthToken = await DoGetOAuthToken();
                if (oAuthToken is null)
                {
                    throw new ThinqApiException("Failed getting oauth token");
                }
                oAuthToken.LastRefresh = DateTime.UtcNow;
                return oAuthToken;
            }
            catch (Exception e)
            {
                if (!silent)
                {
                    Logger.LogError(e, "Exception occured while getting oauth token");
                }
                throw;
            }
        }

        private static string GetBackendUrl(AuthorizeEmpResponse authorizeEmpResponse)
        {
            if (authorizeEmpResponse.RedirectUriParameters is null)
            {
                throw new ArgumentNullException($"Unexpected null value '{nameof(authorizeEmpResponse.RedirectUriParameters)}'");
            }
            return authorizeEmpResponse.RedirectUriParameters.OAuth2BackendUrl;
        }

        private static Dictionary<string, string> GetQueryParams(AuthorizeEmpResponse authorizeEmpResponse)
        {
            if (authorizeEmpResponse.RedirectUriParameters is null)
            {
                throw new ArgumentNullException($"Unexpected null value '{nameof(authorizeEmpResponse.RedirectUriParameters)}'");
            }
            return new Dictionary<string, string>
            {
                { "code", authorizeEmpResponse.RedirectUriParameters.Code },
                { "grant_type", "authorization_code" },
                { "redirect_uri", "lgaccount.lgsmartthinq:/" }
            };
        }
    }
}

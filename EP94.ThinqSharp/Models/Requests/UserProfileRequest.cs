using EP94.ThinqSharp.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models.Requests
{
    internal class UserProfileRequest : OAuthRequest<UserProfileResponse>
    {
        public UserProfileRequest(string accessToken, AuthorizeEmpResponse authorizeEmpResponse, ILoggerFactory loggerFactory)
            : base (HttpMethod.Get, loggerFactory.CreateLogger<UserProfileRequest>(), $"{GetBackendUrl(authorizeEmpResponse)}oauth/1.0/users/profile", GetBody(accessToken), GetExtraHeaders(accessToken))
        {

        }

        public async Task<UserProfile> GetUserProfile(bool silent)
        {
            Logger.LogDebug("Executing get user profile request");
            try
            {
                UserProfileResponse? userProfileResponse = await ExecuteOAuthRequest();
                if (userProfileResponse is null)
                {
                    throw new ThinqApiException("Failed getting user profile response");
                }
                return userProfileResponse.Account;
            }
            catch (Exception e)
            {
                if (!silent)
                {
                    Logger.LogError(e, "Exception occured while getting user profile");
                }
                throw;
            }
        }

        private static Dictionary<string, string> GetExtraHeaders(string accessToken)
        {
            return new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {accessToken}" }
            };
        }

        private static Dictionary<string, string> GetBody(string accessToken)
        {
            return new Dictionary<string, string>
            {
                { "access_code", accessToken }
            };
        }

        private static string GetBackendUrl(AuthorizeEmpResponse authorizeEmpResponse)
        {
            if (authorizeEmpResponse.RedirectUriParameters is null)
            {
                throw new ArgumentNullException($"Unexpected null value '{nameof(authorizeEmpResponse.RedirectUriParameters)}'");
            }
            return authorizeEmpResponse.RedirectUriParameters.OAuth2BackendUrl;
        }
    }
}

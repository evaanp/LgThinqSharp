using EP94.ThinqSharp.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EP94.ThinqSharp.Exceptions;

namespace EP94.ThinqSharp.Models.Requests
{
    internal class PreLoginRequest : RequestBase<PreLoginResult>
    {
        public PreLoginRequest(ILoggerFactory loggerFactory, Gateway gateway, string username, string encryptedPassword)
            : base(HttpMethod.Post, $"{gateway.EmpSpxUri}/preLogin", RequestType.UrlEncoded, loggerFactory.CreateLogger<PreLoginRequest>(), GetBody(username, encryptedPassword), null)
        {

        }

        /// <summary>
        /// Get the prelogin result, which contains an encrypted version of the entered password
        /// </summary>
        /// <param name="silent">Don't log possible errors</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="ThinqApiException"></exception>
        public async Task<PreLoginResult> GetPreLoginResult(bool silent)
        {
            Logger.LogDebug("Getting prelogin result");
            try
            {
                PreLoginResult? preLoginResult = await ExecuteRequestWithResponseAsync();
                if (preLoginResult is null)
                {
                    throw new ThinqApiException("Failed to convert the response to a PreLoginResult instance");
                }
                Logger.LogDebug("Getting prelogin result succeeded");
                return preLoginResult;
            }
            catch (Exception e)
            {
                if (!silent)
                {
                    Logger.LogError(e, "Exception occured while getting prelogin result");
                }
                throw;
            }
        }

        private static Dictionary<string, string> GetBody(string username, string encryptedPassword)
        {
            return new Dictionary<string, string>
            {
                { "user_auth2", encryptedPassword },
                { "log_param", $"login request / user_id : {username} / third_party : null / svc_list : SVC202,SVC710 / 3rd_service : " }
            };
        }
    }
}

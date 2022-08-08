using EP94.ThinqSharp.Exceptions;
using EP94.ThinqSharp.Utils;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models.Requests
{
    internal class AuthorizeEmpRequest : RequestBase<AuthorizeEmpResponse>
    {
        public AuthorizeEmpRequest(ILoggerFactory loggerFactory, Account account, string oauthSecret)
            : base(HttpMethod.Get, $"https://emp-oauth.lgecloud.com/emp/oauth2/authorize/empsession?{GetUrlEncodedParametersString(account)}", RequestType.UrlEncoded, loggerFactory.CreateLogger<AuthorizeEmpRequest>(), null, GetHeaders(account, oauthSecret))
        {

        }

        public async Task<AuthorizeEmpResponse> AuthorizeEmp(bool silent)
        {
            Logger.LogDebug("Executing authorize emp request");
            try
            {
                AuthorizeEmpResponse? response = await ExecuteRequestWithResponseAsync();
                if (response is null)
                {
                    throw new ThinqApiException("Failed to get authorize emp response");
                }
                Logger.LogDebug("Executing authorize emp succeeded");
                Logger.LogDebug("Parsing redirect uri");
                string queryStringJson = JsonConvert.SerializeObject(QueryHelpers.ParseQuery(response.RedirectUri.Query).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.First()));
                AuthorizeEmpResponseRedirectUriParameters? authorizeEmpResponseRedirectUriParameters = JsonConvert.DeserializeObject<AuthorizeEmpResponseRedirectUriParameters>(queryStringJson);
                if (authorizeEmpResponseRedirectUriParameters is null)
                {
                    throw new ThinqApiException($"Failed to parse redirect uri query parameters: {response.RedirectUri}");
                }
                response.RedirectUriParameters = authorizeEmpResponseRedirectUriParameters;
                return response;
            }
            catch (Exception e)
            {
                if (!silent)
                {
                    Logger.LogError(e, "Exception occured while authorizing emp");
                }
                throw;
            }
        }

        private static string GetUrlEncodedParametersString(Account account)
        {
            FormUrlEncodedContent formUrlEncoded = new FormUrlEncodedContent(GetBody(account));
            return formUrlEncoded.ReadAsStringAsync().Result;
        }

        private static Dictionary<string, string> GetHeaders(Account account, string oauthSecret)
        {
            string relativeUrlEncoded = $"/emp/oauth2/authorize/empsession?{GetUrlEncodedParametersString(account)}";

            string timestamp = DateTime.UtcNow.ToRFC822Date();
            byte[] secret = Encoding.UTF8.GetBytes(oauthSecret);
            string messageString = $"{relativeUrlEncoded}\n{timestamp}";
            byte[] message = Encoding.UTF8.GetBytes(messageString);
            byte[] hash = new HMACSHA1(secret).ComputeHash(message);
            string signature = Convert.ToBase64String(hash);
            return new Dictionary<string, string>
            {
                { "lgemp-x-app-key", "LGAO722A02" },
                { "lgemp-x-date", timestamp },
                { "lgemp-x-session-key", account.LoginSessionID },
                { "lgemp-x-signature", signature },
                { "X-Device-Type", "M01" },
                { "X-Device-Platform", "ADR" },
                { "User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36 Edg/93.0.961.44" },
                { "Access-Control-Allow-Origin", "*" },
                { "Accept-Encoding", "gzip, deflate, br" },
                { "Accept-Language", "en-US,en;q=0.9" }
            };
        }

        private static Dictionary<string, string> GetBody(Account account)
        {
            return new Dictionary<string, string>
            {
                { "account_type", account.UserIDType },
                { "client_id", "LGAO221A02" },
                { "country_code", account.Country },
                { "redirect_uri", "lgaccount.lgsmartthinq:/" },
                { "response_type", "code" },
                { "state", "12345" },
                { "username", account.UserID }
            };
        }
    }
}

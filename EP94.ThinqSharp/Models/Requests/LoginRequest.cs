using EP94.ThinqSharp.Exceptions;
using Microsoft.Extensions.Logging;

namespace EP94.ThinqSharp.Models.Requests
{
    internal class LoginRequest : RequestBase<LoginResponse>
    {
        private Dictionary<string, string> _body;
        private Dictionary<string, string> _headers;
        public LoginRequest(ILoggerFactory loggerFactory, Gateway gateway, PreLoginResult preLoginResult, string username)
            : base(HttpMethod.Post, $"{gateway.EmpTermsUri}/emp/v2.0/account/session/{GetUrlEncodedUsername(username)}", RequestType.UrlEncoded, loggerFactory.CreateLogger<LoginRequest>())

        {
            _body = GetBody(preLoginResult);
            _headers = GetHeaders(preLoginResult, gateway);
        }

        private static string GetUrlEncodedUsername(string username)
        {
            return Uri.EscapeDataString(username);
        }

        public async Task<Account> Login(bool silent)
        {
            Logger.LogDebug("Executing login request");
            try
            {
                LoginResponse? loginResponse = await ExecuteRequestWithResponseAsync(_body, _headers);
                if (loginResponse is null)
                {
                    throw new ThinqApiException("Failed to get the account response");
                }
                Logger.LogDebug("Executing login request succeeded");
                return loginResponse.Account;
            }
            catch (Exception e)
            {
                if (!silent)
                {
                    Logger.LogError(e, "Exception occured while executing login request");
                }
                throw;
            }
        }

        private static Dictionary<string, string> GetBody(PreLoginResult preLoginResult)
        {
            return new Dictionary<string, string>
            {
                { "user_auth2", preLoginResult.EncryptedPassword },
                { "password_hash_prameter_flag", "Y" }, // "prameter" is intentional, it won't work without it
                { "svc_list", "SVC202,SVC710" },
                { "inactive_policy", "N" }
            };
        }

        private static Dictionary<string, string> GetHeaders(PreLoginResult preLoginResult, Gateway gateway)
        {
            return new Dictionary<string, string>
            {
                { "Accept", "application/json" },
                { "X-Application-Key", "6V1V8H2BN5P9ZQGOI5DAQ92YZBDO3EK9" },
                { "X-Client-App-Key", "LGAO221A02" },
                { "X-Lge-Svccode", "SVC709" },
                { "X-Device-Type", "M01" },
                { "X-Device-Platform", "ADR" },
                { "X-Device-Language-Type", "IETF" },
                { "X-Device-Publish-Flag", "Y" },
                { "X-Device-Country", gateway.CountryCode },
                { "X-Device-Language", gateway.LanguageCode },
                { "Access-Control-Allow-Origin", "*" },
                { "Accept-Encoding", "gzip, deflate, br" },
                { "Accept-Language", "en-US,en;q=0.9" },
                { "X-Signature", preLoginResult.Signature },
                { "X-Timestamp", preLoginResult.TimeStamp.ToString() },
            };
        }
    }
}

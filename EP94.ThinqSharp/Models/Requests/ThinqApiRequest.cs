using EP94.ThinqSharp.Exceptions;
using Microsoft.Extensions.Logging;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

namespace EP94.ThinqSharp.Models.Requests
{
    internal abstract class ThinqApiRequest<TResult> : RequestBase<ThinqApiResponse<TResult>>
    {
        private ILoggerFactory _loggerFactory;
        private Passport? _passport;
        private object? _body;
        private Dictionary<string, string>? _extraHeaders;
        private string _clientId;

        protected ThinqApiRequest(string clientId, Passport? passport, HttpMethod httpMethod, string url, RequestType requestType, ILogger logger, ILoggerFactory loggerFactory, object? body = null, Dictionary<string, string>? headers = null)
            : base(httpMethod, url, requestType, logger)
        {
            _loggerFactory = loggerFactory;
            _passport = passport;
            _body = body;
            _extraHeaders = headers;
            _clientId = clientId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="failWhenAuthenticationFailed"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="ThinqApiException"></exception>
        protected Task<TResult> ExecuteThinqApiRequestWithResponseAsync()
        {
            return InternalExecuteThinqApiRequestWithResponseAsync(false);
        }

        private async Task<TResult> InternalExecuteThinqApiRequestWithResponseAsync(bool failWhenAuthenticationFailed = false)
        {
            if (_passport is not null && _passport.Token.NeedsRefresh())
            {
                await RefreshOAuthToken(_passport);
            }
            ThinqApiResponse<TResult>? response = await ExecuteRequestWithResponseAsync(_body, GetHeaders(_clientId, _passport, _extraHeaders), false);
            if (response is null)
            {
                throw new ThinqApiException("Request received empty response");
            }
            Logger.LogDebug("Received result code {ResultCode}", response.ResultCode);
            switch (response.ResultCode)
            {
                case ThinqResponseCode.OK:
                    return response.Result;

                case ThinqResponseCode.EMP_AUTHENTICATION_FAILED when !failWhenAuthenticationFailed && _passport is not null:
                    await RefreshOAuthToken(_passport);
                    return await InternalExecuteThinqApiRequestWithResponseAsync(true);

                default:
                    throw new ThinqApiException($"Received error {response.ResultCode} code {(int)response.ResultCode}", response.ResultCode);
            }
        }

        private async Task RefreshOAuthToken(Passport passport)
        {
            Logger.LogInformation("OAuth token needs to be refreshed");
            RefreshOAuthTokenRequest refreshOAuthTokenRequest = new RefreshOAuthTokenRequest(passport, _loggerFactory);
            await refreshOAuthTokenRequest.RefreshOAuthToken(false);
        }

        private static Dictionary<string, string> GetHeaders(string clientId, Passport? passport, Dictionary<string, string>? headers)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "x-client-id", clientId },
                { "x-message-id", new Regex("=*$").Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "") },
                { "x-api-key", "VGhpblEyLjAgU0VSVklDRQ==" },
                { "x-service-code", "SVC202" },
                { "x-service-phase", "OP" },
                { "x-thinq-app-level", "PRD" },
                { "x-thinq-app-os", "ANDROID" },
                { "x-thinq-app-type", "NUTS" },
                { "x-thinq-app-ver", "3.0.1700" },
                { "client_id", clientId },
            };
            if (passport is not null)
            {
                dict.Add("x-country-code", passport.Country);
                dict.Add("x-language-code", passport.Language);
                dict.Add("country_code", passport.Country);
                dict.Add("language_code", passport.Language);
                dict.Add("x-emp-token", passport.Token.AccessToken);
                dict.Add("x-user-no", passport.UserProfile.UserNo);
                dict.Add("Authorization", $"Bearer {passport.Token.AccessToken}");
            }
            if (headers is not null)
            {
                foreach (var item in headers)
                {
                    dict.Add(item.Key, item.Value);
                }
            }
            return dict;
        }
    }
}

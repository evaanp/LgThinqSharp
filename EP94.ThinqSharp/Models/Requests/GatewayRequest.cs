using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EP94.ThinqSharp.Exceptions;
using System.Globalization;

namespace EP94.ThinqSharp.Models.Requests
{
    internal class GatewayRequest : ThinqApiRequest<Gateway>
    {
        public GatewayRequest(string clientId, CultureInfo cultureInfo, ILoggerFactory loggerFactory)
            : base(clientId, null, HttpMethod.Get, "https://route.lgthinq.com:46030/v1/service/application/gateway-uri", RequestType.UrlEncoded, loggerFactory.CreateLogger<GatewayRequest>(), loggerFactory, null, GetHeaders(cultureInfo.TwoLetterISOLanguageName.ToUpper(), cultureInfo.Name))
        {
        }

        public GatewayRequest(string clientId, Passport passport, ILoggerFactory loggerFactory)
            : base (clientId, passport, HttpMethod.Get, "https://route.lgthinq.com:46030/v1/service/application/gateway-uri", RequestType.UrlEncoded, loggerFactory.CreateLogger<GatewayRequest>(), loggerFactory, null, null)
        { }

        private static Dictionary<string, string> GetHeaders(string countryCode, string languageCode)
        {
            return new Dictionary<string, string>
            {
                { "country_code", countryCode },
                { "language_code", languageCode },
                { "x-country-code", countryCode },
                { "x-language-code",languageCode }
            };
        }

        /// <summary>
        /// Get the gateway object from the thinq api
        /// </summary>
        /// <param name="silent">Don't log possible errors</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="ThinqApiException"></exception>
        public async Task<Gateway> GetGatewayAsync(bool silent)
        {
            Logger.LogDebug("Executing gateway request");
            try
            {
                Gateway gateway = await ExecuteThinqApiRequestWithResponseAsync();
                Logger.LogDebug("Gateway request succeeded");
                return gateway;
            }
            catch (Exception e)
            {
                if (!silent)
                {
                    Logger.LogError(e, "Exception occured while requesting gateway");
                }
                throw;
            }
        }
    }
}

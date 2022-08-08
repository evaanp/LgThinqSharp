using EP94.ThinqSharp.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models.Requests
{
    internal class OAuthRequest<TReturn> : RequestBase<TReturn>
    {
        public OAuthRequest(HttpMethod httpMethod, ILogger logger, string url, Dictionary<string, string> queryParams, Dictionary<string, string>? extraHeaders = null)
            : base(httpMethod, $"{url}?{GetUrlEncodedParamsString(queryParams)}", RequestType.UrlEncoded, logger, queryParams, GetHeaders(url, queryParams, extraHeaders))
        {
        }

        private static string GetUrlEncodedParamsString(Dictionary<string, string> queryParams)
        {
            FormUrlEncodedContent formUrlEncoded = new FormUrlEncodedContent(queryParams);
            return formUrlEncoded.ReadAsStringAsync().Result;
        }

        private static Dictionary<string, string> GetHeaders(string url, Dictionary<string, string> queryParams, Dictionary<string, string>? extraHeaders)
        {
            Uri uri = new Uri(url);
            string relativeUrlEncoded = $"{uri.LocalPath}?{GetUrlEncodedParamsString(queryParams)}";

            string timestamp = DateTime.UtcNow.ToRFC822Date();
            byte[] secret = Encoding.UTF8.GetBytes("c053c2a6ddeb7ad97cb0eed0dcb31cf8");
            string messageString = $"{relativeUrlEncoded}\n{timestamp}";
            byte[] message = Encoding.UTF8.GetBytes(messageString);
            byte[] hash = new HMACSHA1(secret).ComputeHash(message);
            string signature = Convert.ToBase64String(hash);

            Dictionary<string, string> result = new Dictionary<string, string>
            {
                { "Accept", "application/json; charset=UTF-8" },
                { "x-lge-oauth-signature", signature },
                { "x-lge-oauth-date", timestamp },
                { "x-lge-appkey", "LGAO221A02" }
            };
            if (extraHeaders is not null)
            {
                foreach (var kvp in extraHeaders)
                {
                    result.Add(kvp.Key, kvp.Value);
                }
            }
            return result;
        }
    }
}

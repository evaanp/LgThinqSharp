using EP94.ThinqSharp.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EP94.ThinqSharp.Models.Requests
{
    internal abstract class RequestBase
    {
        protected HttpMethod HttpMethod { get; }
        protected string Url { get; }
        protected RequestType RequestType { get; }
        protected object? Body { get; }
        protected Dictionary<string, string>? Headers { get; }
        protected ILogger Logger { get; }

        protected RequestBase(HttpMethod httpMethod, string url, RequestType requestType, ILogger logger, object? body, Dictionary<string, string>? headers)
        {
            HttpMethod = httpMethod;
            Url = url;
            RequestType = requestType;
            Body = body;
            Headers = headers;
            Logger = logger;
        }

        /// <summary>
        /// Execute the request
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException">When the request fails or the statuscode is not success this exception is thrown</exception>
        protected async Task ExecuteRequestAsync(bool throwOnNonSuccessStatusCode = true)
        {
            HttpResponseMessage response = await RequestInternalAsync();
            using HttpContent content = response.Content;
            string stringContent = await content.ReadAsStringAsync();
            try
            {
                if (throwOnNonSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(stringContent, e);
            }
        }

        /// <summary>
        /// Execute the request
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        protected async Task<HttpResponseMessage> RequestInternalAsync()
        {
            using HttpClient httpClient = new HttpClient();

            HttpRequestMessage? httpRequestMessage = null;
            try
            {
                if (Body is not null)
                {
                    HttpContent httpContent = RequestType switch
                    {
                        RequestType.UrlEncoded => new FormUrlEncodedContent(Body.ToDictionary()),
                        RequestType.Json => new StringContent(JsonConvert.SerializeObject(Body), null, "application/json"),
                        _ => throw new NotSupportedException(RequestType.ToString())
                    };
                    httpRequestMessage = new HttpRequestMessage(HttpMethod, Url)
                    {
                        Content = httpContent,
                    };
                }
                else
                {
                    httpRequestMessage = new HttpRequestMessage(HttpMethod, Url);
                }

                if (Headers is not null)
                {
                    foreach (var (key, value) in Headers)
                    {
                        httpRequestMessage.Headers.Add(key, value?.ToString());
                    }
                }
                Logger.LogDebug("Executing {HttpMethod} request to url {Url} with request type {RequestType}", HttpMethod, Url, RequestType);
                return await httpClient.SendAsync(httpRequestMessage);
            }
            finally
            {
                httpRequestMessage?.Dispose();
            }
        }

        private FormUrlEncodedContent FormUrlEncodedContent(object body)
        {
            FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(body.ToDictionary());
            formUrlEncodedContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            formUrlEncodedContent.Headers.ContentType.CharSet = "UTF-8";
            return formUrlEncodedContent;
        }
    }
    internal abstract class RequestBase<TReturn> : RequestBase
    {
        protected RequestBase(HttpMethod httpMethod, string url, RequestType requestType, ILogger logger, object? body, Dictionary<string, string>? headers)
            : base(httpMethod, url, requestType, logger, body, headers)
        {
        }
        /// <summary>
        /// Execute the request and try to convert the response to the generic return type
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException">When the request fails or the statuscode is not success this exception is thrown</exception>
        protected async Task<TReturn?> ExecuteRequestWithResponseAsync(bool throwOnNonSuccessStatusCode = true)
        {
            using HttpResponseMessage response = await RequestInternalAsync();
            using HttpContent content = response.Content;
            string stringContent = await content.ReadAsStringAsync();
            try
            {
                if (throwOnNonSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(stringContent, e, e.StatusCode);
            }
            return JsonConvert.DeserializeObject<TReturn>(stringContent);
        }
    }
}

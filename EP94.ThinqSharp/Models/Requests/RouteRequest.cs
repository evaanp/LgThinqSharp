using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models.Requests
{
    internal class RouteRequest : ThinqApiRequest<Route>
    {
        public RouteRequest(string clientId, Passport passport, ILoggerFactory loggerFactory)
            : base (clientId, passport, HttpMethod.Get, "https://common.lgthinq.com/route", RequestType.UrlEncoded, loggerFactory.CreateLogger<RouteRequest>(), loggerFactory)
        {

        }

        public async Task<Route> GetRouteAsync(bool silent)
        {
            Logger.LogDebug("Executing get route request");
            try
            {
                Route route = await ExecuteThinqApiRequestWithResponseAsync();
                Logger.LogDebug("Get route request succeeded");
                return route;
            }
            catch (Exception e)
            {
                if (!silent)
                {
                    Logger.LogError(e, "Exception occured while requesting route");
                }
                throw;
            }
        }
    }
}

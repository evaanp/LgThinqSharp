using EP94.ThinqSharp.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models.Requests
{
    internal class GetSecretOAuthKeyRequest : RequestBase<SecretOAuthKeyResponse>
    {
        public GetSecretOAuthKeyRequest(Gateway gateway, ILoggerFactory loggerFactory)
            : base (HttpMethod.Get, $"{gateway.EmpSpxUri}/searchKey?key_name=OAUTH_SECRETKEY&sever_type=OP", RequestType.UrlEncoded, loggerFactory.CreateLogger<GetSecretOAuthKeyRequest>(), null, null)
        {

        }

        public async Task<string> GetSecretOAuthKey(bool silent)
        {
            Logger.LogDebug("Getting secret oauth key");
            try
            {
                SecretOAuthKeyResponse? response = await ExecuteRequestWithResponseAsync();
                Logger.LogDebug("Getting secret oauth key succeeded");
                if (response is null)
                {
                    throw new ThinqApiException("Failed getting secret oauth key");
                }
                return response.ReturnData;
            }
            catch (Exception e)
            {
                if (!silent)
                {
                    Logger.LogError(e, "Exception occured while getting secret oauth key");
                }
                throw;
            }
        }
    }
}

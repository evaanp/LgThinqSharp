using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models
{
    internal class SecretOAuthKeyResponse
    {
        public string ReturnData { get; set; }

        public SecretOAuthKeyResponse(string returnData)
        {
            ReturnData = returnData;
        }
    }
}

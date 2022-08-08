using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models
{
    internal record RequestResponse(HttpStatusCode StatusCode)
    {

    }
    internal record RequestResponse<T>(HttpStatusCode StatusCode, T? Result) : RequestResponse(StatusCode)
    {

    }
}

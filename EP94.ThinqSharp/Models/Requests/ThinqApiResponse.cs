using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models.Requests
{
    internal record ThinqApiResponse<T>(ThinqResponseCode ResultCode, T Result)
    {
    }
}

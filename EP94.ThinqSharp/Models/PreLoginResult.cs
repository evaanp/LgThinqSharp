using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models
{
    internal record PreLoginResult(
        [property: JsonProperty("tStamp")]
        long TimeStamp,
        [property: JsonProperty("signature")]
        string Signature,
        [property: JsonProperty("encrypted_pw")]
        string EncryptedPassword
        )
    { }
}

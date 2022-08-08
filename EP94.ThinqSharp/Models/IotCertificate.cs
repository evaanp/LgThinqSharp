using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models
{
    internal class IotCertificate
    {
        [JsonIgnore]
        public X509Certificate2 CertificatePemCertificate => new X509Certificate2(Encoding.UTF8.GetBytes(CertificatePem));

        public string CertificatePem { get; set; }
        public List<string> Subscriptions { get; set; }

        public IotCertificate(string certificatePem, List<string> subscriptions)
        {
            CertificatePem = certificatePem;
            Subscriptions = subscriptions;
        }
    }
}

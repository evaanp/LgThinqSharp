using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models
{
    internal class Gateway
    {
        public string CountryCode { get; set; }
        public string LanguageCode { get; set; }
        public string Thinq1Uri { get; set; }
        public string Thinq2Uri { get; set; }
        public string EmpUri { get; set; }
        public string EmpTermsUri { get; set; }
        public string EmpSpxUri { get; set; }

        public Gateway(string countryCode, string languageCode, string thinq1Uri, string thinq2Uri, string empUri, string empTermsUri, string empSpxUri)
        {
            CountryCode = countryCode;
            LanguageCode = languageCode;
            Thinq1Uri = thinq1Uri;
            Thinq2Uri = thinq2Uri;
            EmpUri = empUri;
            EmpTermsUri = empTermsUri;
            EmpSpxUri = empSpxUri;
        }
    }
}

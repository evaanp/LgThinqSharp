using System;
using System.Collections.Generic;
using System.Text;

namespace EP94.ThinqSharp.Models
{
    public class Passport
    {
        public OAuthToken Token { get; set; }
        public UserProfile UserProfile { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }

        public Passport(OAuthToken token, UserProfile userProfile, string country, string language)
        {
            Token = token;
            UserProfile = userProfile;
            Country = country;
            Language = language;
        }
    }
}

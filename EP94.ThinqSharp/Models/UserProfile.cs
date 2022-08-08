using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EP94.ThinqSharp.Models
{
    public class UserProfile
    {
        public string UserNo { get; set; }
        public string UserId { get; set; }

        public UserProfile(string userNo, string userId)
        {
            UserNo = userNo;
            UserId = userId;
        }
    }
}

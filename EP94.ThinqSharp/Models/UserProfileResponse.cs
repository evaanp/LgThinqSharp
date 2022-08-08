using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models
{
    internal class UserProfileResponse
    {
        public UserProfile Account { get; set; }

        public UserProfileResponse(UserProfile account)
        {
            Account = account;
        }
    }
}

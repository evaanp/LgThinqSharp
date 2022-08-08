using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models
{
    internal class LoginResponse
    {
        public Account Account { get; set; }

        public LoginResponse(Account account)
        {
            Account = account;
        }
    }
}

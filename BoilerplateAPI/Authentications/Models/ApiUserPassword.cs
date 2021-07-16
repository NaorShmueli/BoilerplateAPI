using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentications.Models
{
    internal class ApiUserPassword
    {
        public string User { get; private set; }
        public string Password { get; private set; }
        public ApiUserPassword(string user, string password)
        {
            User = user;
            Password = password;
        }
    }
}

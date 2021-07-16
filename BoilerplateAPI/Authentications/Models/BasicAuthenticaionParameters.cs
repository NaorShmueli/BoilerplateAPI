using Authentications.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentications.Models
{
    internal class BasicAuthenticaionParameters : IAuthenticaionParameter
    {
        public BasicAuthenticaionParameters(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
        public string UserName { get; private set; }
        public string Password { get; private set; }
    }
}

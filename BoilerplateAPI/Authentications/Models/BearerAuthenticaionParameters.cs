using Authentications.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentications.Models
{
    internal class BearerAuthenticaionParameters : IAuthenticaionParameter
    {
        public BearerAuthenticaionParameters(string token)
        {
            Token = token;
        }
        public string Token { get; private set; }
    }
}

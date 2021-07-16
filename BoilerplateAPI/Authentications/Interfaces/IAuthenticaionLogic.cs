using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentications.Interfaces
{
    internal interface IAuthenticaionLogic<T> where  T : IAuthenticaionParameter
    {
        internal bool Authenticate(T parameters);
    }
}

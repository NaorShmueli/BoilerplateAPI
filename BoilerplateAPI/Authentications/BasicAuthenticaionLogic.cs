using Authentications.Interfaces;
using Authentications.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentications
{
    public class BasicAuthenticaionLogic : IAuthenticaionLogic<BasicAuthenticaionParameters>
    {
        private ApiUserPassword _apiUserPassword;
        private ILogger<BasicAuthenticaionLogic> _logger;
        public BasicAuthenticaionLogic(ILogger<BasicAuthenticaionLogic> logger, IConfiguration configuration)
        {
            _logger = logger;
            _apiUserPassword = new ApiUserPassword(configuration["ApiUser"], configuration["ApiPassword"]);
        }

        bool IAuthenticaionLogic<BasicAuthenticaionParameters>.Authenticate(BasicAuthenticaionParameters parameters)
        {
            var result = _apiUserPassword.User == parameters.UserName && _apiUserPassword.Password == parameters.Password;
            if (!result)
            {
                _logger.LogError($"Api Authenticaion failed");
            }
            return result;
        }


    }
}

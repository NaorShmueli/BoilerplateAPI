using Authentications.Interfaces;
using Authentications.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentications
{
    class BearerAuthenticationLogic : IAuthenticaionLogic<BearerAuthenticaionParameters>
    {
        private ILogger<BearerAuthenticationLogic> _logger;
        private IConfiguration _configuration;

        public BearerAuthenticationLogic(ILogger<BearerAuthenticationLogic> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        bool IAuthenticaionLogic<BearerAuthenticaionParameters>.Authenticate(BearerAuthenticaionParameters parameters)
        {
            var result = true;
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Secret"]);
            tokenHandler.ValidateToken(parameters.Token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero,
                ValidAudience = "MyAudience",
                ValidIssuer = "MyIssuer",
                ValidateLifetime = true,

            }, out SecurityToken validatedToken);

            return result;
        }


    }
}

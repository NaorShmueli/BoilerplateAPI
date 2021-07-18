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
                LifetimeValidator = (before, expires, token,parameters) =>
                {
                    var cloneParams = parameters.Clone();
                    cloneParams.LifetimeValidator = null;
                    Microsoft.IdentityModel.Tokens.Validators.ValidateLifetime(before, expires, token, cloneParams);
                    result = AdditionalValidation(token as JwtSecurityToken);
                    return result;
                }

            }, out SecurityToken validatedToken);

            return result;
        }

        private bool AdditionalValidation(JwtSecurityToken jwtSecurityToken)
        {
            var upperBarrier = new DateTimeOffset(DateTime.UtcNow.AddSeconds(_configuration.GetValue<int>("IssuedAtUpperBarrier"))).ToUniversalTime().ToUnixTimeSeconds();
            var lowerBarrier = new DateTimeOffset(DateTime.UtcNow.AddSeconds(_configuration.GetValue<int>("IssuedAtLowerBarrier"))).ToUniversalTime().ToUnixTimeSeconds();
            var issuedAt = new DateTimeOffset(jwtSecurityToken.Payload.IssuedAt).ToUniversalTime().ToUnixTimeSeconds();
            var issueValidation = issuedAt > lowerBarrier && issuedAt < upperBarrier;
            return issueValidation;
        }
    }
}

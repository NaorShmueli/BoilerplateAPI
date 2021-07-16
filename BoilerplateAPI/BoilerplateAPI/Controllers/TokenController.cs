using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BoilerplateAPI.Controllers
{
    [Route("[Controller]/api")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet("token")]
        public IActionResult GetToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var sharedKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Secret"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddSeconds(250),
                IssuedAt = DateTime.UtcNow.AddSeconds(50),
                NotBefore = DateTime.UtcNow.AddSeconds(10),
                Issuer = "MyIssuer",
                Audience = "MyAudience",
                SigningCredentials = new SigningCredentials(sharedKey, SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                }),

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenStr = tokenHandler.WriteToken(token);
            return Ok(tokenStr);
        }
    }
}

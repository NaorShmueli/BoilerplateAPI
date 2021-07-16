using Authentications;
using Microsoft.AspNetCore.Authorization;
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
    public class TestController : ControllerBase
    {
        [Authorize(AuthenticationSchemes = AuthenticaionScheme.Bearer)]
        [HttpGet("test/bearer")]
        public IActionResult TestBearer()
        {
            return Ok("Authentication passed");
        }

        [Authorize(AuthenticationSchemes = AuthenticaionScheme.Basic)]
        [HttpGet("test/basic")]
        public IActionResult TestBasic()
        {
            return Ok("Authentication passed");
        }

    }
}

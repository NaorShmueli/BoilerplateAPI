using Authentications.Interfaces;
using Authentications.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Authentications
{
    public class BearerAuthenticaionHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAuthenticaionLogic<BearerAuthenticaionParameters> _authenticaionLogic;
        private readonly ILogger<BearerAuthenticaionHandler> _logger;
        public BearerAuthenticaionHandler(ILogger<BearerAuthenticaionHandler> logger, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory loggerFactory, UrlEncoder urlEncoder, ISystemClock clock, IConfiguration configuration) : base(options, loggerFactory, urlEncoder, clock)
        {
            _authenticaionLogic = new BearerAuthenticationLogic(new Logger<BearerAuthenticationLogic>(loggerFactory), configuration);
            _logger = logger;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                if (!Request.Headers.ContainsKey("Authorization"))
                {
                    var msg = "Missing Bearer header";
                    _logger.LogError(msg);
                    return Task.FromResult(AuthenticateResult.Fail(msg));
                }
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentials = authHeader.Parameter.Split("Bearer", 1);
                var token = credentials[0];
                var parameters = new BearerAuthenticaionParameters(token);
                return Authenticate(parameters);
            }
            catch (Exception ex)
            {
                var msg = "Error occur during authentications";
                _logger.LogError(msg, ex);
                return Task.FromResult(AuthenticateResult.Fail(msg));

            }



        }

        private Task<AuthenticateResult> Authenticate(BearerAuthenticaionParameters parameters)
        {
            var isAuthenticated = _authenticaionLogic.Authenticate(parameters);
            if (!isAuthenticated)
            {
                var msg = "Invalid token";
                _logger.LogError(msg);
                return Task.FromResult(AuthenticateResult.Fail(msg));
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,parameters.Token)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));

        }
    }
}

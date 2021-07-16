using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Authentications.Interfaces;
using Authentications.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Authentications
{
    public class BasicAuthenticaionHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAuthenticaionLogic<BasicAuthenticaionParameters> _authenticaionLogic;
        private readonly ILogger<BasicAuthenticaionHandler> _logger;
        public BasicAuthenticaionHandler(ILogger<BasicAuthenticaionHandler> logger,IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory loggerFactory, UrlEncoder urlEncoder, ISystemClock clock, IConfiguration configuration) : base(options, loggerFactory, urlEncoder, clock)
        {
            _authenticaionLogic = new BasicAuthenticaionLogic(new Logger<BasicAuthenticaionLogic>(loggerFactory), configuration);
            _logger = logger;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                if (!Request.Headers.ContainsKey("Authorization"))
                {
                    var msg = "Missing authorization header";
                    _logger.LogError(msg);
                    return Task.FromResult(AuthenticateResult.Fail(msg));
                }
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(":", 2);
                var userName = credentials[0];
                var password = credentials[1];
                var parameters = new BasicAuthenticaionParameters(userName, password);
                return Authenticate(parameters);
            }
            catch (Exception ex)
            {
                var msg = "Error occur during authentications";
                _logger.LogError(msg, ex);
                return Task.FromResult(AuthenticateResult.Fail(msg));

            }



        }

        private Task<AuthenticateResult> Authenticate(BasicAuthenticaionParameters parameters)
        {
            var isAuthenticated = _authenticaionLogic.Authenticate(parameters);
            if (!isAuthenticated)
            {
                var msg = "Invalid user name or password";
                _logger.LogError(msg);
                return Task.FromResult(AuthenticateResult.Fail(msg));
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,parameters.UserName)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new  AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));

        }
    }
}

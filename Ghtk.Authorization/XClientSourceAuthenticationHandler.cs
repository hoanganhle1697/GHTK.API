using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Ghtk.Authorization
{
    public class XClientSourceAuthenticationHandler : AuthenticationHandler<XClientSourceAuthenticationHandlerOptions>
    {
        public XClientSourceAuthenticationHandler(IOptionsMonitor<XClientSourceAuthenticationHandlerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            var clientSource = Context.Request.Headers["XClientSource"];
            var token = Context.Request.Headers["Token"];


            if (clientSource.Count == 0)
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Token header."));
            }

            if (token.Count == 0)
            {
                

            }
            var clientSourceValue = clientSource.FirstOrDefault();
            var tokenValue = token.FirstOrDefault();

            if (!string.IsNullOrEmpty(clientSourceValue) &&
                !string.IsNullOrEmpty(tokenValue) &&
                VerifyClient(clientSourceValue, tokenValue))
            {



                var identity = new ClaimsIdentity(Scheme.Name);

                identity.AddClaim(new Claim("XClientSource", clientSourceValue));

                var principal = new ClaimsPrincipal(identity); 
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            else
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Token header value."));
            }
        }

        private bool VerifyClient(string clientSourceValue, string tokenValue)
        {
            if (!Validate(tokenValue, out var token,out var principal))
            {
                return false;
            }

            if (clientSourceValue != principal?.FindFirst(ClaimTypes.Name)?.Value)
            {
                return false;
            }
            if (!Options.ClientValidator(clientSourceValue, token, principal))
            {
                return false;
            }
            return true;
        }

        private bool Validate(string tokenValue, out SecurityToken? token,out ClaimsPrincipal? claimsPrincipal)
        {
            var handler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(Options.IssuerSigningKey);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                claimsPrincipal=handler.ValidateToken(tokenValue,tokenValidationParameters, out token);

                return true;
            }
            catch (Exception)
            {
                token = null;
                claimsPrincipal = null;
                return false;
            }
        }
    }
}

using Microsoft.AspNetCore.Authentication;

namespace Ghtk.Authorization
{
    public static class XClientSourceAuthenticationHandlerExtensions
    {
        public static AuthenticationBuilder AddXClientSource(this AuthenticationBuilder builder,Action<XClientSourceAuthenticationHandlerOptions> configureOptions)
            =>builder.AddScheme<Ghtk.Authorization.XClientSourceAuthenticationHandlerOptions, Ghtk.Authorization.XClientSourceAuthenticationHandler>("XClientSource", configureOptions);

    }
}
 
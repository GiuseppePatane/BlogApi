using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Blog.Api.Auth;

public class XUserAuthenticationHandler : AuthenticationHandler<XUserAuthenticationOptions>
{
    public XUserAuthenticationHandler(
        IOptionsMonitor<XUserAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }


    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        throw new NotImplementedException("fake auth flow");
    }
}
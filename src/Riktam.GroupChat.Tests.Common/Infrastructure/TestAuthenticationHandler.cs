using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Riktam.GroupChat.Tests.Common.Infrastructure;

[ExcludeFromCodeCoverage]
public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
{
    public TestAuthenticationHandler(IOptionsMonitor<TestAuthenticationOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock) :
        base(options, logger, encoder, clock)
    {
    }

    public static string TestScheme => "Test";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authenticationTicket = new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(Options.Claims, TestScheme)), TestScheme);

        return Task.FromResult(Options.ShouldSucceed
            ? AuthenticateResult.Success(authenticationTicket)
            : AuthenticateResult.Fail("Test Authentication Failed."));
    }
}

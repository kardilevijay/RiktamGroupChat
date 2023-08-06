using Microsoft.AspNetCore.Authentication;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Riktam.GroupChat.Tests.Common.Infrastructure;

[ExcludeFromCodeCoverage]
public class TestAuthenticationOptions : AuthenticationSchemeOptions
{
    public bool ShouldSucceed { get; set; } = true;
    public List<Claim> Claims { get; set; } = new List<Claim>();

    public TestAuthenticationOptions()
    {
    }
}

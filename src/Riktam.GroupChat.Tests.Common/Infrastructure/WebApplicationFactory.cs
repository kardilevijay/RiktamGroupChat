using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Riktam.GroupChat.Tests.Common.Infrastructure;

[ExcludeFromCodeCoverage]
public sealed class WebApplicationFactory : ConfigurableWebApplicationFactory<Program>
{
    private WebApplicationFactory(Action<IConfigurationBuilder>? testConfiguration)
         : base(testConfiguration)
    {
    }

    public static WebApplicationFactory CreateForBehavioralTests(Action<IConfigurationBuilder>? testConfiguration = null)
        => new(builder =>
            {
                testConfiguration?.Invoke(builder);
            });

    public static WebApplicationFactory CreateForNonBehavioralTests()
        => new(null);
}
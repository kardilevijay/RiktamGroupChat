using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Riktam.GroupChat.Tests.Common.Infrastructure;

[ExcludeFromCodeCoverage]
public class ConfigurableWebApplicationFactory<TEntryPoint> :
    WebApplicationFactory<TEntryPoint>,
    IConfigurableWebApplicationFactory where TEntryPoint : class
{
    private readonly List<Action<IServiceCollection>> _testServiceConfigurations = new();
    private readonly Action<IConfigurationBuilder>? _testAppConfiguration;

    public ConfigurableWebApplicationFactory(Action<IConfigurationBuilder>? testAppConfiguration = null)
    {
        _testAppConfiguration = testAppConfiguration;
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _ = builder.ConfigureTestServices(sc =>
        {
            foreach (var testServiceConfiguration in _testServiceConfigurations)
            {
                testServiceConfiguration(sc);
            }
        });

        if (_testAppConfiguration != null)
        {
            builder.ConfigureAppConfiguration(_testAppConfiguration);
        }
    }

    public void ConfigureTestServices(Action<IServiceCollection> action)
    {
        _testServiceConfigurations.Add(action);
    }
}
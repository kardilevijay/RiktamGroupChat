using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Riktam.GroupChat.Tests.Common.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace Riktam.GroupChat.Tests.Common.Extensions;

[ExcludeFromCodeCoverage]
public static class ConfigurableWebApplicationFactoryExtensions
{
    public static Mock<T> Mock<T>(this IConfigurableWebApplicationFactory factory) where T : class
    {
        var mock = new Mock<T>();

        factory.ConfigureTestServices(services =>
        {
            services.ReplaceAllWithSingleton(mock.Object);
        });

        return mock;
    }

    public static void AddTestAuthenticationHandler(this IConfigurableWebApplicationFactory factory,
       TestAuthenticationOptions testAuthenticationOptions)
    {
        factory.ConfigureTestServices(services =>
        {
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = TestAuthenticationHandler.TestScheme;
            })
            .AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(
                    TestAuthenticationHandler.TestScheme,
                    "Test Auth", o =>
                    {
                        if (testAuthenticationOptions == null)
                        {
                            o.ShouldSucceed = true;
                        }
                        else
                        {
                            o.ShouldSucceed = testAuthenticationOptions.ShouldSucceed;
                            o.Claims = testAuthenticationOptions.Claims;
                        }
                    });

            var policyBuilder = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(TestAuthenticationHandler.TestScheme);

            var authFilter = new AuthorizeFilter(policyBuilder.Build());

            services.AddMvc(options =>
            {
                var existingAuthFilter = options.Filters.Single(b => b is AuthorizeFilter) as AuthorizeFilter;
                options.Filters.Remove(existingAuthFilter!);
                options.Filters.Add(authFilter);
            });
        });
    }
}

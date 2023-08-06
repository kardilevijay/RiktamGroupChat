using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace Riktam.GroupChat.Tests.Common.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static Mock<T> Mock<T>(this IServiceCollection services) where T : class
    {
        var mock = new Mock<T>();

        services.ReplaceAllWithSingleton(mock.Object);

        return mock;
    }
    public static IServiceCollection ReplaceAllWithSingleton<TService>(this IServiceCollection services, TService service)
        where TService : class
    {
        services.RemoveAll<TService>();
        services.AddSingleton(service);
        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace Riktam.GroupChat.Tests.Common.Infrastructure;

public interface IConfigurableWebApplicationFactory
{
    void ConfigureTestServices(Action<IServiceCollection> action);
}
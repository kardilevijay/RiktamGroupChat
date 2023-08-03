using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riktam.GroupChat.Domain.Services;

namespace Riktam.GroupChat.Domain;

public static class DomainConfigrator
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IHashGenerator, SHA256HashGenerator>();
    }
}
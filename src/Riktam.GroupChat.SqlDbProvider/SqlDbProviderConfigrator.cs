using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riktam.GroupChat.Domain.Providers;
using Riktam.GroupChat.SqlDbProvider.Implementation;
using Riktam.GroupChat.SqlDbProvider.Infrastructure;

namespace Riktam.GroupChat.SqlDbProvider;

public static class SqlDbProviderConfigrator
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GroupChatDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddTransient<IUserRepository, UserRepository>();
    }
}

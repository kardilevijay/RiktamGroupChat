using Microsoft.EntityFrameworkCore;
using Riktam.GroupChat.SqlDbProvider.Models;
using Group = Riktam.GroupChat.SqlDbProvider.Models.Group;

namespace Riktam.GroupChat.SqlDbProvider.Infrastructure;

public class GroupChatDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMessage> GroupMessages { get; set; }
    public DbSet<GroupMembership> GroupMemberships { get; set; }

    public GroupChatDbContext(DbContextOptions<GroupChatDbContext> options) : base(options)
    {
    }
}

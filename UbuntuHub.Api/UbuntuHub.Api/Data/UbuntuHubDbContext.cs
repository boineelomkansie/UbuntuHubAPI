using Microsoft.EntityFrameworkCore;
using UbuntuHub.Api.Models;

namespace UbuntuHub.Api.Data
{
    public class UbuntuHubDbContext : DbContext
    {
        public UbuntuHubDbContext(
            DbContextOptions<UbuntuHubDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Interest> Interests { get; set; }

        public DbSet<Message> Messages { get; set; }
    }
}
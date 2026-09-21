using Microsoft.EntityFrameworkCore;
using PushPushServer.Models;

namespace PushPushServer.Data
{
    public class GameDBContext : DbContext
    {
        public GameDBContext(DbContextOptions<GameDBContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameDBContext).Assembly);
        }
    }
}

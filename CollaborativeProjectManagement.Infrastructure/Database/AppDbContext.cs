using Microsoft.EntityFrameworkCore;
using CollaborativeProjectManagement.Domain.Entities.Auth;

namespace CollaborativeProjectManagement.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
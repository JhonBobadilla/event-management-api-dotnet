using Microsoft.EntityFrameworkCore;
using EventManagement.Domain.Entities;

namespace EventManagement.Infrastructure.Persistence
{
    /// DbContext principal para la app, gestionando entidades y conexión a PostgreSQL.
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Attendee> Attendees => Set<Attendee>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
        }
    }
}

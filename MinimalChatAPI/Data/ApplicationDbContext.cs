using Microsoft.EntityFrameworkCore;
using MinimalChatAPI.Models.Domain;

namespace MinimalChatAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //configure table names
            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}

using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Utils
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<Apartment> Apartment { get; set; }
        public DbSet<Criteria> Criteria { get; set; }
        public DbSet<District> District { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<Realtor> Realtor { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<Score> Score { get; set; }
        public DbSet<Models.Type> Type { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=realtor-codefirst;Username=postgres;Password=12345678");
        }
    }
}

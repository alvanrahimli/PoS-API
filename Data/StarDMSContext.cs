using Microsoft.EntityFrameworkCore;
using StarDMS.Models;
using StarDMS.Utilities;

namespace StarDMS.Data
{
    public class StarDMSContext : DbContext
    {
        public StarDMSContext(DbContextOptions<StarDMSContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder builder) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Seed data
            builder.Seed();

            // Fluent API
        }

        // Tables
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Firm> Firms { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}
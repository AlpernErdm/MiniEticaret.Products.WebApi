using Microsoft.EntityFrameworkCore;
using MiniEticaret.Products.WebApi.Models;

namespace MiniEticaret.Products.WebApi.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("money"); //decimal veritabanında money tipinde tutulcak
        }
    }
}

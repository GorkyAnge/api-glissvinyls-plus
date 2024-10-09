using glissvinyls_plus.Models;
using Microsoft.EntityFrameworkCore;

namespace glissvinyls_plus.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {


        }
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Price)
                      .HasColumnType("decimal(18, 2)"); // Especificar el tipo de columna
            });
        }


    }
}

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
        public DbSet<Category> Categories { get; set; }
        public DbSet<EntryDetail> EntryDetails { get; set; }
        public DbSet<ExitDetail> ExitDetails { get; set; }
        public DbSet<InventoryEntry> InventoryEntries { get; set; }
        public DbSet<InventoryExit> InventoryExits { get; set; }
        public DbSet<MovementHistory> MovementHistories { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }


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

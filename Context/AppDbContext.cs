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
    }
}

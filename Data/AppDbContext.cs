using Microsoft.EntityFrameworkCore;
using ProductCrud.Models;

namespace ProductCrud.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using ProductCrud.Models;

namespace ProductCrud.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products => Set<Product>();
    }
}

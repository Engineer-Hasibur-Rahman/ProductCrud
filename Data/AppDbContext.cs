using Microsoft.EntityFrameworkCore;
using ProductCrud.Models;

namespace ProductCrud.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
                base.OnModelCreating(modelBuilder);

            // Product -Category relationship
             modelBuilder.Entity<Product>()
                .HasOne(p => p.Category) // product has one category
                .WithMany(c => c.Products) // category has many products
                .HasForeignKey(p => p.CategoryId) // FK in Product table
                .OnDelete(DeleteBehavior.Restrict); // restrict delete
        }

    }
}

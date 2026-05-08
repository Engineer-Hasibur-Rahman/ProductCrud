using Microsoft.EntityFrameworkCore;
using ProductCrud.Data;
using ProductCrud.Models;

namespace ProductCrud.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        // Dependency Injection
        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {            
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                   .Include(p => p.Category)
                   .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> UpdateProductAsync(int id, Product product)
        {
            var existing = await _context.Products.FindAsync(id);

            if (existing == null)
                return false;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
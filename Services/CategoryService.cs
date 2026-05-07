using Microsoft.EntityFrameworkCore;
using ProductCrud.Data;
using ProductCrud.Models;

namespace ProductCrud.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        // Dependency Injection
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync(); 
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> UpdateCategoryAsync(int id, Category category)
        {
            var existing = await _context.Categories.FindAsync(id);

            if (existing == null)
                return false;

            existing.Name = category.Name;
            existing.Description = category.Description;
          
            // update image only if exists
            if (!string.IsNullOrEmpty(category.Image))
            {
                existing.Image = category.Image;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var response = await _context.Categories.FindAsync(id);

            if (response == null)
                return false;

            _context.Categories.Remove(response);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}

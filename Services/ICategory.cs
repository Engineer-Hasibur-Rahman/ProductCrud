using ProductCrud.Models;
using System.Threading.Tasks;   

namespace ProductCrud.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        Task<Category?> GetCategoryByIdAsync(int id);

        Task<Category> CreateCategoryAsync(Category category);

        Task<bool> UpdateCategoryAsync(int id, Category category);

        Task<bool> DeleteCategoryAsync(int id);
    }
}

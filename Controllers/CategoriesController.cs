using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductCrud.DTOs;
using ProductCrud.Models;
using ProductCrud.Services;
using System.Threading.Tasks;

namespace ProductCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        //static List<Product> products = new List<Product> 
        //    {
        //        new Product { Id = 1, Name = "Product 1", Description = "HP Laptop", Price = 10.99m },
        //        new Product { Id = 2, Name = "Product 2", Description = "Dell Laptop", Price = 19.99m },
        //        new Product { Id = 3, Name = "Product 3", Description = "Apple Laptop", Price = 5.99m }
        //    };

        // dependency injection
        private readonly ICategoryService _service;
        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        // Get all Categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {            
            var response = await _service.GetAllCategoriesAsync();

            return Ok(response);
        }

        // Get a Category by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _service.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var result = new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Image = category.Image
            };

            return Ok(result);
        }


        // create category 
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                Image = dto.Image
            };

            var result = await _service.CreateCategoryAsync(category);

            var response = new CategoryReadDto
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                Image = result.Image
            };

            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = result.Id },
                response
            );
        }



        // Update a Category
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryUpdateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                Image = dto.Image
            };

            var result = await _service.UpdateCategoryAsync(id, category);

            if (!result)
                return NotFound();

            return NoContent();
        }

        // delete a Category
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _service.DeleteCategoryAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        } 

    }
}

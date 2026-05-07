using Microsoft.AspNetCore.Mvc;
using ProductCrud.DTOs;
using ProductCrud.Models;
using ProductCrud.Services;

namespace ProductCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly IWebHostEnvironment _environment;

        public CategoriesController(
            ICategoryService service,
            IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _service.GetAllCategoriesAsync();

            var response = categories.Select(category => new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Image = category.Image,
                ImageUrl = category.Image != null
            ? $"{Request.Scheme}://{Request.Host}/uploads/categories/{category.Image}"
            : null
            });

            return Ok(response);
        }

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
                Image = category.Image,
               ImageUrl = category.Image != null
            ? $"{Request.Scheme}://{Request.Host}/uploads/categories/{category.Image}"
            : null
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateDto dto)
        {
            string? imageName = null;

            if (dto.Image != null)
            {
                imageName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);

                var webRootPath = _environment.WebRootPath;
                if (string.IsNullOrEmpty(webRootPath))
                {
                    webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }

                var folderPath = Path.Combine(webRootPath, "uploads", "categories");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var imagePath = Path.Combine(folderPath, imageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }
            }

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                Image = imageName
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryUpdateDto dto)
        {
            string? imageName = null;

            if (dto.Image != null)
            {
                imageName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);

                var webRootPath = _environment.WebRootPath;
                if (string.IsNullOrEmpty(webRootPath))
                {
                    webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }

                var folderPath = Path.Combine(webRootPath, "uploads", "categories");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var imagePath = Path.Combine(folderPath, imageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }
            }

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                Image = imageName
            };

            var result = await _service.UpdateCategoryAsync(id, category);

            if (!result)
                return NotFound();

            return NoContent();
        }

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
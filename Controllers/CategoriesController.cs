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
        private readonly IFileStorageService _fileStorageService;

        public CategoriesController(
            ICategoryService service,
            IFileStorageService fileStorageService)
        {
            _service = service;
            _fileStorageService = fileStorageService;
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
                ImageUrl = _fileStorageService.GetSingleFileUrl(category.Image, Request, "categories")
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateDto dto)
        {
            var imageName = await _fileStorageService.SingleFileUploadAsync(dto.Image, "categories");

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
                Image = result.Image,
                ImageUrl = _fileStorageService.GetSingleFileUrl(result.Image, Request, "categories")
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
            var existingCategory = await _service.GetCategoryByIdAsync(id);

            if (existingCategory == null)
                return NotFound();

            if (dto.Image != null)
            {
                await _fileStorageService.DeleteSingleFileAsync(existingCategory.Image, "categories");
                existingCategory.Image = await _fileStorageService.SingleFileUploadAsync(dto.Image, "categories");
            }

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            var result = await _service.UpdateCategoryAsync(id, category);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            // get existing category
            var category = await _service.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            // delete image file
            await _fileStorageService.DeleteSingleFileAsync(
                category.Image,
                "categories"
            );

            // delete database record
            var result = await _service.DeleteCategoryAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
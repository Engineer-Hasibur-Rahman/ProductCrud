using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCrud.DTOs;
using ProductCrud.Models;
using ProductCrud.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IFileStorageService _fileStorageService;

        public ProductsController(
            IProductService productService,
            IFileStorageService fileStorageService)
        {
            _productService = productService;
            _fileStorageService = fileStorageService;
        }

        // Get all products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();

            var result = products.Select(p => new ProductReadDto
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                Name = p.Name,
                Slug = p.Slug,
                Image = p.Image,
                ImageUrl = _fileStorageService.GetSingleFileUrl(p.Image, Request, "products"),
                Description = p.Description,
                Price = p.Price,
                DisountPrice = p.DisountPrice,
                Quantity = p.Quantity,
                 // Category Relation 
               CategoryName = p.Category != null ? p.Category.Name : null

            }).ToList();


            return Ok(result);
        }

        // Get a product by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            var result = new ProductReadDto
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Name = product.Name,
                Slug = product.Slug,
                Image = product.Image,
                ImageUrl = _fileStorageService.GetSingleFileUrl(product.Image, Request, "products"),
                Description = product.Description,
                Price = product.Price,
                DisountPrice = product.DisountPrice,
                Quantity = product.Quantity,
                // Category Relation 
                CategoryName = product.Category != null ? product.Category.Name : null
            };

            return Ok(result);
        }

        // Create product
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Product name is required.");

            // generate slug
            var slug = dto.Name.Trim().ToLower().Replace(" ", "-");

            // upload image
            var imageName = await _fileStorageService.SingleFileUploadAsync(dto.Image, "products");

            var product = new Product
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                Slug = slug,
                Image = imageName,
                Description = dto.Description,
                Price = dto.Price,
                DisountPrice = dto.DisountPrice,
                Quantity = dto.Quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await _productService.CreateProductAsync(product);

            var response = new ProductReadDto
            {
                Id = result.Id,
                CategoryId = result.CategoryId,
                Name = result.Name,
                Slug = result.Slug,
                Image = result.Image,
                ImageUrl = _fileStorageService.GetSingleFileUrl(result.Image, Request, "products"),
                Description = result.Description,
                Price = result.Price,
                DisountPrice = result.DisountPrice,
                Quantity = result.Quantity
            };

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = result.Id },
                response
            );
        }

        // Update a product
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateDto dto)
        {
            var existingProduct = await _productService.GetProductByIdAsync(id);

            if (existingProduct == null)
                return NotFound();         


            // update image if new image comes
            if (dto.Image != null)
            {
                await _fileStorageService.DeleteSingleFileAsync(existingProduct.Image, "products");

                existingProduct.Image = await _fileStorageService.SingleFileUploadAsync(dto.Image, "products");
            }

            existingProduct.CategoryId = dto.CategoryId;
            existingProduct.Name = dto.Name ?? existingProduct.Name;
            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                existingProduct.Slug = dto.Name.Trim().ToLower().Replace(" ", "-");
            }
            existingProduct.Description = dto.Description ?? existingProduct.Description;
            existingProduct.Price = dto.Price ?? existingProduct.Price;
            existingProduct.DisountPrice = dto.DisountPrice ?? existingProduct.DisountPrice;
            existingProduct.Quantity = dto.Quantity ?? existingProduct.Quantity;
            existingProduct.CategoryId = dto.CategoryId ?? existingProduct.CategoryId;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            var result = await _productService.UpdateProductAsync(id, existingProduct);

            if (!result)
                return NotFound();

            return NoContent();
        }

        // Delete a product
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existingProduct = await _productService.GetProductByIdAsync(id);

            if (existingProduct == null)
                return NotFound();

            await _fileStorageService.DeleteSingleFileAsync(existingProduct.Image, "products");

            var result = await _productService.DeleteProductAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
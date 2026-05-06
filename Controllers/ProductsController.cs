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
    public class ProductsController : ControllerBase
    {

        //static List<Product> products = new List<Product> 
        //    {
        //        new Product { Id = 1, Name = "Product 1", Description = "HP Laptop", Price = 10.99m },
        //        new Product { Id = 2, Name = "Product 2", Description = "Dell Laptop", Price = 19.99m },
        //        new Product { Id = 3, Name = "Product 3", Description = "Apple Laptop", Price = 5.99m }
        //    };

        // dependency injection
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // Get all products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {            
            var products = await _productService.GetAllProductsAsync();

            return Ok(products);
        }

        // Get a product by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var result = new ProductReadDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };

            return Ok(result);
        }


        // create product 
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            };

            var result = await _productService.CreateProductAsync(product);

            var response = new ProductReadDto
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                Price = result.Price
            };

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = result.Id },
                response
            );
        }



        // Update a product
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductUpdateDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            };

            var result = await _productService.UpdateProductAsync(id, product);

            if (!result)
                return NotFound();

            return NoContent();
        }

        // delete a product
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        } 

    }
}

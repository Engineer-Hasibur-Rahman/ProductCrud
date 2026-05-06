using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }


        // create product 
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            var result = await _productService.CreateProductAsync(product);

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = result.Id },
                result
            );
        }


        // Update a product
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            var result = await _productService.UpdateProductAsync(id, product);

            if (!result)
                return NotFound();

            return NoContent();

        }

        // delete a product
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        } 

    }
}

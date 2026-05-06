using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCrud.Models;

namespace ProductCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        static List<Product> products = new List<Product> 
            {
                new Product { Id = 1, Name = "Product 1", Description = "HP Laptop", Price = 10.99m },
                new Product { Id = 2, Name = "Product 2", Description = "Dell Laptop", Price = 19.99m },
                new Product { Id = 3, Name = "Product 3", Description = "Apple Laptop", Price = 5.99m }
            };

        // Get all products
        [HttpGet]
        public IActionResult GetProducts()
        {            

            return Ok(products);
        }

        // Get a product by id
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetProductById(int id) 
        {
            var response = products.FirstOrDefault(p => p.Id == id);

            if(response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        // Update a product
        [HttpPut]
        [Route("{id}")]
        public IActionResult PutProduct(Product product)
        {
            var existingProduct = products.FirstOrDefault(p => p.Id == product.Id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            // update data
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;

            return NoContent();

        }

        // delete a product
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteProduct(int id) 
        { 
            var existingProduct = products.FirstOrDefault(p => p.Id == id);

            if (existingProduct == null) {

                return NotFound();
            }

            // delete data
            products.Remove(existingProduct);

            return NoContent();
        } 

    }
}

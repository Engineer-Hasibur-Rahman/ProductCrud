using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ProductCrud.DTOs
{
    public class ProductCreateDto
    {
        public int? CategoryId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        public decimal DisountPrice { get; set; }
        public int Quantity { get; set; } = 0;

        public IFormFile? Image { get; set; }
    

    }
}

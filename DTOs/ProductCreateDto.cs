using System.ComponentModel.DataAnnotations;

namespace ProductCrud.DTOs
{
    public class ProductCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

    }
}

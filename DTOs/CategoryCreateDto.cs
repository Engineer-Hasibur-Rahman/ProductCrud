using System.ComponentModel.DataAnnotations;

namespace ProductCrud.DTOs
{
    public class CategoryCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string Image { get; set; }

    }
}

using System.Numerics;

namespace ProductCrud.DTOs
{
    public class ProductUpdateDto
    {
        public int? CategoryId { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }

        public decimal? Price { get; set; }
        public decimal? DisountPrice { get; set; }

        public int? Quantity { get; set; }

        public IFormFile? Image { get; set; }
    }


}

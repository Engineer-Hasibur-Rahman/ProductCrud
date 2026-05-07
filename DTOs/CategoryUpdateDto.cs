namespace ProductCrud.DTOs
{
    public class CategoryUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}

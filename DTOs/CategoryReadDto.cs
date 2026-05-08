namespace ProductCrud.DTOs
{
    public class CategoryReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string? ImageUrl { get; set; }
    }
}

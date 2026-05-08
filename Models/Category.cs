namespace ProductCrud.Models
{
    public class Category
    {
        public  int Id { get; set; }
        public  string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // (One Category → Many Products)
        public ICollection<Product> Products { get; set; }

    }

}

namespace ProductCrud.Models
{
    public class Product
    {
        public  int Id { get; set; }

        // Basic Info
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }

        // Pricing
        public decimal Price { get; set; }
        public decimal DisountPrice { get; set; }

        // Inventory 
        public int Quantity { get; set; }
        public string Image { get; set; }
        public string ImageUrl { get; set; }

        // status 
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; }       

        // Audit 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Category Relation
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}

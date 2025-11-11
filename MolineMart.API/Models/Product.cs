namespace MolineMart.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }

        // ✅ New optional fields
        public string Category { get; set; }
        public string Storage { get; set; }
        public string Color { get; set; }
        public string Warranty { get; set; }
    }
}

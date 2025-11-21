namespace MolineMart.API.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }
        public string Storage { get; set; }
        public string Warranty { get; set; }
        public string ImageUrl { get; set; }
    }

}

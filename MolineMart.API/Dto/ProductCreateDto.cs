namespace MolineMart.API.Dto
{
    public class ProductCreateDto
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile Image { get; set; }
    }

}

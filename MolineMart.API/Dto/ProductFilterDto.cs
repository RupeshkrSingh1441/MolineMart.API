namespace MolineMart.API.Dto
{
    public class ProductFilterDto
    {
        public string Search { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}

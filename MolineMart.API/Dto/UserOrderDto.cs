namespace MolineMart.API.Dto
{
    public class UserOrderDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime OrderDate { get; set; }

        public string ProductModel { get; set; }
        public string ProductImage { get; set; }
    }
}

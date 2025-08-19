namespace MolineMart.API.Dto
{
    public class CreateOrderRequest
    {
        public decimal Amount { get; set; } // Amount in INR
        public string ReceiptId { get; set; } // Optional receipt ID for the order
    }
}

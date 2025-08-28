namespace MolineMart.API.Dto
{
    public class CreateOrderRequest
    {
        public decimal Amount { get; set; } // Amount in INR
        public string ReceiptId { get; set; } // Optional receipt ID for the order
        public string Email { get; set; }
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public class CartLine
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }
}

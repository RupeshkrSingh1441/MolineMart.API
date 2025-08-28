namespace MolineMart.API.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;
        public string RazorpayPaymentId { get; set; } = default!;
        public string RazorpayOrderId { get; set; } = default!;
        public string RazorpaySignature { get; set; } = default!;
        public string Provider { get; set; } = "Razorpay"; // Default provider is Razorpay
        public int AmountPaise { get; set; }
        public string Currency { get; set; } = "INR"; // Default currency is INR
        public string Status { get; set; } = "captured"; // Default status is captured
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Default to current UTC time

    }
}

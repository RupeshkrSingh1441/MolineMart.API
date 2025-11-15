
namespace MolineMart.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string RazorpayOrderId { get; set; }
        public string RazorpayPaymentId { get; set; }

        // Foreign Key
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow; // Default to current UTC time
        public decimal Amount { get; set; }
        public string Email { get; set; }
        public string Status { get; set; } ="Pending"; // Default status is Pending

        public ICollection<OrderItem> Items { get; set; }
        public Payment Payment { get; set; }
    }
}

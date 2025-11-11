namespace MolineMart.API.Models
{
    public class DeliveryAvailability
    {
        public int Id { get; set; }
        public string Pincode { get; set; }
        public bool IsAvailable { get; set; }
        public int EstimatedDays { get; set; }
    }

}

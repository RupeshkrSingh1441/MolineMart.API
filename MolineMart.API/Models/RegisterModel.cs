namespace MolineMart.API.Models
{
    public class RegisterModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }   
        public string Password { get; set; }

        // Address fields
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }
}

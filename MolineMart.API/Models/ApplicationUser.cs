using Microsoft.AspNetCore.Identity;

namespace MolineMart.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        // Address fields
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }
}

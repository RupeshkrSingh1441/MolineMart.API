using System.Security.Principal;

namespace MolineMart.API.Dto
{
    public class VerifySignatureRequest
    {
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string Signature { get; set; } // The signature to verify
    }
}

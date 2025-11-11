namespace MolineMart.API.Models
{
    public class ProductFeature
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string FeatureTitle { get; set; }
        public string FeatureDescription { get; set; }

        public Product Product { get; set; }
    }

}

namespace MolineMart.API.Models
{
    public class RelatedProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int RelatedProductId { get; set; }

        public Product Product { get; set; }
    }

}

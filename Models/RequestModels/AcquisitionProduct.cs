namespace glissvinyls_plus.Models.RequestModels
{
    public class AcquisitionProduct
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public string Image { get; set; } = null!;
    }
}

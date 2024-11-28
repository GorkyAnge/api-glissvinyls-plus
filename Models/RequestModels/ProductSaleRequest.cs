namespace glissvinyls_plus.Models.RequestModels
{
    public class ProductSaleRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }
    }
}

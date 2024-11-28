namespace glissvinyls_plus.Models.RequestModels
{
    public class SaleRequest
    {
        public int ClientId { get; set; }
        public int WarehouseId { get; set; }
        public List<ProductSaleRequest> Products { get; set; }
    }
}

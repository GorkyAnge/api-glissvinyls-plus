namespace glissvinyls_plus.Models.RequestModels
{
    public class AcquisitionRequest
    {
        public int SupplierId { get; set; }
        public int WarehouseId { get; set; }
        public List<AcquisitionProduct> Products { get; set; } = new List<AcquisitionProduct>();
    }
}

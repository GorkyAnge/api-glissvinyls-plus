using System.ComponentModel.DataAnnotations;

namespace glissvinyls_plus.Models
{
    public class ExitDetail
    {
        [Key]
        public int ExitDetailId { get; set; }
        public int ExitId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float SalePrice { get; set; }
    }

}

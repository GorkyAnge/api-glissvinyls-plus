using System.ComponentModel.DataAnnotations;

namespace glissvinyls_plus.Models
{
    public class InventoryExit
    {
        [Key]
        public int ExitId { get; set; }
        public DateTime ExitDate { get; set; }
        public int ClientId { get; set; }
        public float TotalExit { get; set; }
    }

}

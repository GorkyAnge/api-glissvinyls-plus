using System.ComponentModel.DataAnnotations;

namespace glissvinyls_plus.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

}

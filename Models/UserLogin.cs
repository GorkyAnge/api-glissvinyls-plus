using System.ComponentModel.DataAnnotations;

namespace glissvinyls_plus.Models
{
    public class UserLogin
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

}

using System.ComponentModel.DataAnnotations;

namespace StarDMS.Models.Dtos
{
    public class SellerLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
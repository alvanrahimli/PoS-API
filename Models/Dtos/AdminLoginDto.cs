using System.ComponentModel.DataAnnotations;

namespace StarDMS.Models.Dtos
{
    public class AdminLoginDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
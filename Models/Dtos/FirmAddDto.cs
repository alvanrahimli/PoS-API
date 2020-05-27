using System.ComponentModel.DataAnnotations;

namespace StarDMS.Models.Dtos
{
    public class FirmAddDto
    {
        [Required]
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Details { get; set; }
    }
}
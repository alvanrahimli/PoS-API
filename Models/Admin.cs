using System;
using System.ComponentModel.DataAnnotations;

namespace StarDMS.Models
{
    public class Admin
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

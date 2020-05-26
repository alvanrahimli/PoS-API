using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarDMS.Models
{
    public class Firm
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Details { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
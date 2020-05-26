using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarDMS.Models
{
    public class Seller
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }
        [StringLength(500)]
        public string Details { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
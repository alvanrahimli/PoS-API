using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarDMS.Models
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid IssuerId { get; set; } // Seller Id
        [Required]
        public DateTime IssueDate { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Description { get; set; } // Special formatted string
        [ForeignKey("IssuerId")]
        public Seller Issuer { get; set; }
    }
}
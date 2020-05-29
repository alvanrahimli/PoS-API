using System;
using System.ComponentModel.DataAnnotations;

namespace StarDMS.Models
{
    public class Product
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Barcode { get; set; }
        [Required]
        public string Name { get; set; }
        public int Count { get; set; }
        public DateTime EntryDate { get; set; }
        [StringLength(300)]
        public string Details { get; set; }
        [Required]
        public Guid FirmId { get; set; }
        public int PurchasePrice { get; set; }
        public int SalePrice { get; set; }
        [Range(0, 100)]
        public int Discount { get; set; }
        public Firm Firm { get; set; }
    }
}
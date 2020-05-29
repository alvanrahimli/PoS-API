using System;
using System.ComponentModel.DataAnnotations;

namespace StarDMS.Models.Dtos
{
    public class ProductAddDto
    {
        [Required]
        [StringLength(13)]
        public string Barcode { get; set; }
        [Required]
        public string Name { get; set; }
        public int Count { get; set; }
        [StringLength(300)]
        public string Details { get; set; }
        [Required]
        public DateTime EntryDate { get; set; }
        [Required]
        public Guid FirmId { get; set; }
        public int PurchasePrice { get; set; }
        public int SalePrice { get; set; }
    }
}
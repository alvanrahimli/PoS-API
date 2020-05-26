using System;

namespace StarDMS.Models.Dtos
{
    public class ProductReturnDto
    {
        public Guid Id { get; set; }
        public string Barcode { get; set; }
        public DateTime EntryDate { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string Details { get; set; }
        public Guid FirmId { get; set; }
        public string FirmName { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
    }
}
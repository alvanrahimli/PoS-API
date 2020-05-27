using System;
using Microsoft.EntityFrameworkCore;
using StarDMS.Models;

namespace StarDMS.Utilities
{
    public static class Seeder
    {
        public static void Seed(this ModelBuilder builder)
        {
            // Ids
            var alvanId = Guid.NewGuid(); // admin
            var samilId = Guid.NewGuid(); // admin
            var samirId = Guid.NewGuid();
            var cola = Guid.NewGuid();
            var fanta = Guid.NewGuid();
            var qatiq = Guid.NewGuid();
            var cocacola = Guid.NewGuid();
            var seba = Guid.NewGuid();

            // Data
            builder.Entity<Admin>().HasData(
                new Admin()
                {
                    Id = alvanId,
                    Name = "Alvan Rahimli",
                    Password = "alvan12345"
                },
                new Admin()
                {
                    Id = samilId,
                    Name = "Samil",
                    Password = "samil12345"
                }
            );

            builder.Entity<Product>().HasData(
                new Product()
                {
                    Id = cola,
                    Barcode = "1234567891234",
                    Details = "Kola haqda details",
                    Discount = 0,
                    EntryDate = DateTime.Now.AddDays(1),
                    FirmId = cocacola,
                    Name = "Kola 1L",
                    PurchasePrice = 0.9m,
                    SalePrice = 1m,
                    Count = 150
                },
                new Product()
                {
                    Id = fanta,
                    Barcode = "2345678912345",
                    Details = "Fanta haqda details",
                    Discount = 0,
                    EntryDate = DateTime.Now.AddDays(2),
                    FirmId = cocacola,
                    Name = "Fanta 2.5L",
                    PurchasePrice = 1.62m,
                    SalePrice = 1.7m,
                    Count = 220
                },
                new Product()
                {
                    Id = qatiq,
                    Barcode = "3456789123456",
                    Details = "Esl kend qatigi",
                    Discount = 0,
                    EntryDate = DateTime.Now.AddDays(2),
                    FirmId = seba,
                    Name = "Qatiq 1kq",
                    PurchasePrice = 1.1m,
                    SalePrice = 1.2m,
                    Count = 12
                }
            );

            builder.Entity<Firm>().HasData(
                new Firm()
                {
                    Id = cocacola,
                    Name = "Koka-kola",
                    Contact = "+994511234567",
                    Details = "Test details about kokakola"
                },
                new Firm()
                {
                    Id = seba,
                    Name = "Seba",
                    Contact = "+994512345678",
                    Details = ""
                }
            );

            builder.Entity<Seller>().HasData(
                new Seller()
                {
                    Id = samirId,
                    Details = "Ayliq 400man",
                    Email = "samir@star.az",
                    Name = "Samir Hasanov",
                    Password = "samir12345"
                }
            );
        }
    }
}
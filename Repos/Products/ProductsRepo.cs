using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarDMS.Data;
using StarDMS.Models;
using StarDMS.Models.Dtos;
using StarDMS.Utilities;

namespace StarDMS.Repos.Products
{
    public class ProductsRepo : IProductsRepo
    {
        private readonly StarDMSContext _context;

        public ProductsRepo(StarDMSContext context)
        {
            this._context = context;
        }

        public async Task<RepoResponse<ProductReturnDto>> GetProduct(Guid id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.Firm)
                .Select(p => new ProductReturnDto()
                {
                    Id = p.Id,
                    Barcode = p.Barcode,
                    Details = String.IsNullOrEmpty(p.Details) ? "Qeyd olunmayıb" : p.Details,
                    FirmId = p.FirmId,
                    FirmName = p.Firm.Name,
                    Name = p.Name,
                    PurchasePrice = p.PurchasePrice,
                    SalePrice = p.SalePrice,
                    EntryDate = p.EntryDate,
                    Count = p.Count
                })
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (product == null)
            {
                return new RepoResponse<ProductReturnDto>()
                {
                    Content = null,
                    IsSucces = false,
                    Message = "Verilmiş id ilə məhsul tapılmadı."
                };
            }

            return new RepoResponse<ProductReturnDto>()
            {
                Content = product,
                IsSucces = true,
                Message = "Verilmiş id ilə məhsul tapıldı."
            };
        }

        public async Task<RepoResponse<ProductReturnDto>> GetProduct(string barcode)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.Firm)
                .Select(p => new ProductReturnDto()
                {
                    Id = p.Id,
                    Barcode = p.Barcode,
                    Details = String.IsNullOrEmpty(p.Details) ? "Qeyd olunmayıb" : p.Details,
                    FirmId = p.FirmId,
                    FirmName = p.Firm.Name,
                    Name = p.Name,
                    PurchasePrice = p.PurchasePrice,
                    SalePrice = p.SalePrice,
                    EntryDate = p.EntryDate,
                    Count = p.Count
                })
                .FirstOrDefaultAsync(p => p.Barcode == barcode.Trim());
            
            if (product == null)
            {
                return new RepoResponse<ProductReturnDto>()
                {
                    Content = null,
                    IsSucces = false,
                    Message = "Daxil edilmiş barkod ilə məhsul tapılmadı."
                };
            }

            return new RepoResponse<ProductReturnDto>()
            {
                Content = product,
                IsSucces = true,
                Message = "1 ədəd məhsul tapıldı."
            };
        }

        public async Task<RepoResponse<List<ProductReturnDto>>> GetProducts(int rq, int c)
        {
            var products = await _context.Products
                .AsNoTracking()
                .Include(p => p.Firm)
                .Select(p => new ProductReturnDto()
                {
                    Id = p.Id,
                    Barcode = p.Barcode,
                    Details = String.IsNullOrEmpty(p.Details) ? "Qeyd olunmayıb" : p.Details,
                    FirmId = p.FirmId,
                    FirmName = p.Firm.Name,
                    Name = p.Name,
                    PurchasePrice = p.PurchasePrice,
                    SalePrice = p.SalePrice,
                    EntryDate = p.EntryDate,
                    Count = p.Count
                })
                .OrderBy(p => p.EntryDate)
                .Skip((rq - 1) * c)
                .Take(c)
                .ToListAsync();

            if (products == null)
            {
                return new RepoResponse<List<ProductReturnDto>>()
                {
                    Content = null,
                    IsSucces = false,
                    Message = "Heç bir məhsul tapılmadı."
                };
            }

            return new RepoResponse<List<ProductReturnDto>>()
            {
                Content = products,
                IsSucces = true,
                Message = $"{products.Count} məhsul yükləndi."
            };
        }

        public async Task<RepoResponse<List<ProductAddDto>>> AddMultipleProducts(List<ProductAddDto> newProducts)
        {
            var failedProducts = new List<ProductAddDto>();
            foreach (var product in newProducts)
            {
                var singleRes = await AddProduct(product);
                if (!singleRes.IsSucces)
                {
                    failedProducts.Add(product);
                    continue;
                }
            }

            if (failedProducts.Count > 0)
            {
                return new RepoResponse<List<ProductAddDto>>()
                {
                    Content = failedProducts,
                    IsSucces = false,
                    Message = "Bəzi məhsullar əlavə oluna bilmədi"
                };
            }

            return new RepoResponse<List<ProductAddDto>>()
            {
                Content = null,
                IsSucces = true,
                Message = "Bütün məhsullar bazaya əlavə olundu."
            };
        }

        public async Task<RepoResponse<ProductReturnDto>> AddProduct(ProductAddDto np)
        {
            var oldP = await GetProduct(np.Barcode);
            if (oldP.IsSucces)
            {
                return new RepoResponse<ProductReturnDto>()
                {
                    Content = oldP.Content,
                    IsSucces = false,
                    Message = "Bu barkodla bazada məhsul mövcuddur."
                };
            }

            var newProduct = new Product()
            {
                Id = Guid.NewGuid(),
                Barcode = np.Barcode,
                Details = np.Details,
                Discount = 0,
                EntryDate = np.EntryDate,
                FirmId = np.FirmId,
                Name = np.Name,
                PurchasePrice = np.PurchasePrice,
                SalePrice = np.SalePrice
            };

            await _context.AddAsync(newProduct);
            var dbResult = await _context.SaveChangesAsync();

            if (dbResult > 0)
            {
                return new RepoResponse<ProductReturnDto>()
                {
                    Content = (await GetProduct(newProduct.Id)).Content,
                    IsSucces = true,
                    Message = "Məhsul bazaya əlavə olundu."
                };
            }

            return new RepoResponse<ProductReturnDto>()
            {
                Content = null,
                IsSucces = false,
                Message = "Məhsulu əlavə edərkən səhv baş verdi."
            };
        }

        public async Task<RepoResponse<int>> DeleteProduct(Guid id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (product == null)
            {
                return new RepoResponse<int>()
                {
                    Content = 0,
                    IsSucces = false,
                    Message = "Verilmiş id ilə məhsul tapılmadı."
                };
            }

            _context.Products.Remove(product);
            var dbResult = await _context.SaveChangesAsync();

            if (dbResult > 0)
            {
                return new RepoResponse<int>()
                {
                    Content = dbResult,
                    IsSucces = true,
                    Message = "Məhsul bazadan silindi."
                };
            }
            
            return new RepoResponse<int>()
            {
                Content = 0,
                IsSucces = false,
                Message = "Məhsulu silərkən səhv baş verdi. Yenidən cəhd edin."
            };
        }

        public async Task<RepoResponse<int>> DeleteProduct(string barcode)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Barcode == barcode);
            
            if (product == null)
            {
                return new RepoResponse<int>()
                {
                    Content = 0,
                    IsSucces = false,
                    Message = "Verilmiş barkod ilə məhsul tapılmadı."
                };
            }

            _context.Products.Remove(product);
            var dbResult = await _context.SaveChangesAsync();

            if (dbResult > 0)
            {
                return new RepoResponse<int>()
                {
                    Content = dbResult,
                    IsSucces = true,
                    Message = "Məhsul bazadan silindi."
                };
            }
            
            return new RepoResponse<int>()
            {
                Content = 0,
                IsSucces = false,
                Message = "Məhsulu silərkən səhv baş verdi. Yenidən cəhd edin."
            };
        }

        public async Task<RepoResponse<ProductReturnDto>> UpdateProduct(Product np)
        {
            var oldProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == np.Id);
            
            if (oldProduct == null)
            {
                return new RepoResponse<ProductReturnDto>()
                {
                    Content = null,
                    IsSucces = false,
                    Message = "Verilmiş id ilə məhsul tapılmadı."
                };
            }

            oldProduct.Barcode = np.Barcode;
            oldProduct.Details = np.Details;
            oldProduct.Discount = np.Discount;
            oldProduct.EntryDate = np.EntryDate;
            oldProduct.FirmId = np.FirmId;
            oldProduct.Name = np.Name;
            oldProduct.PurchasePrice = np.PurchasePrice;
            oldProduct.SalePrice = np.SalePrice;
            oldProduct.Count = np.Count;

            var dbRes = await _context.SaveChangesAsync();

            if (dbRes > 0)
            {
                var updated = await GetProduct(np.Id);
                if (updated.IsSucces)
                {
                    return new RepoResponse<ProductReturnDto>()
                    {
                        Content = updated.Content,
                        IsSucces = true,
                        Message = "Məhsul məlumatları yeniləndi."
                    };
                }
            }

            return new RepoResponse<ProductReturnDto>()
            {
                Content = null,
                IsSucces = false,
                Message = "Baza ilə bağlı problem yaşandı. Yenidən cəhd edin."
            };
        }

        public async Task<RepoResponse<List<ProductReturnDto>>> GetEmptyStocks()
        {
            var products = await _context.Products
                .AsNoTracking()
                .Include(p => p.Firm)
                .Where(p => p.Count == 0)
                .Select(p => new ProductReturnDto()
                {
                    Id = p.Id,
                    Barcode = p.Barcode,
                    Details = String.IsNullOrEmpty(p.Details) ? "Qeyd olunmayıb" : p.Details,
                    FirmId = p.FirmId,
                    FirmName = p.Firm.Name,
                    Name = p.Name,
                    PurchasePrice = p.PurchasePrice,
                    SalePrice = p.SalePrice,
                    EntryDate = p.EntryDate,
                    Count = p.Count
                })
                .OrderBy(p => p.EntryDate)
                .ToListAsync();

            if (products == null)
            {
                return new RepoResponse<List<ProductReturnDto>>()
                {
                    Content = null,
                    IsSucces = false,
                    Message = "Heç bir məhsul tapılmadı."
                };
            }

            return new RepoResponse<List<ProductReturnDto>>()
            {
                Content = products,
                IsSucces = true,
                Message = $"{products.Count} məhsul yükləndi."
            };
        }
    }
}
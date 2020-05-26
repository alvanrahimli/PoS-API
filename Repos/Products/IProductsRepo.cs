using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarDMS.Models;
using StarDMS.Models.Dtos;
using StarDMS.Utilities;

namespace StarDMS.Repos.Products
{
    public interface IProductsRepo
    {
        Task<RepoResponse<ProductReturnDto>> GetProduct(string barcode);
        Task<RepoResponse<ProductReturnDto>> GetProduct(Guid id);
        Task<RepoResponse<List<ProductReturnDto>>> GetProducts(int rq, int c);
        Task<RepoResponse<ProductReturnDto>> AddProduct(ProductAddDto newProduct);
        Task<RepoResponse<List<ProductAddDto>>> AddMultipleProducts(List<ProductAddDto> newProducts);
        Task<RepoResponse<ProductReturnDto>> UpdateProduct(Product updatedProduct);
        Task<RepoResponse<int>> DeleteProduct(Guid id);
        Task<RepoResponse<int>> DeleteProduct(string barcode);
        Task<RepoResponse<List<ProductReturnDto>>> GetEmptyStocks();
    }
}
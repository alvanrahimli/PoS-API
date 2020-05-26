using System.Threading.Tasks;
using StarDMS.Data;
using StarDMS.Models;
using StarDMS.Models.Dtos;
using StarDMS.Utilities;

namespace StarDMS.Repos.Auth
{
    public interface IAuthRepo
    {
        Task<RepoResponse<Seller>> Register(SellerRegisterDto seller);
        Task<RepoResponse<Seller>> Login(SellerLoginDto seller);
        Task<RepoResponse<Admin>> AdminLogin(string name, string pass);
        
    }
}
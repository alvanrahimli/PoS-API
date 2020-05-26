using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StarDMS.Data;
using StarDMS.Models;
using StarDMS.Models.Dtos;
using StarDMS.Utilities;

namespace StarDMS.Repos.Auth
{
    public class AuthRepo : IAuthRepo
    {
        private readonly IConfiguration _config;
        private readonly StarDMSContext _context;
        public AuthRepo(StarDMSContext context, IConfiguration config)
        {
            this._config = config;
            this._context = context;
        }

        public async Task<RepoResponse<Admin>> AdminLogin(string name, string pass)
        {
            var admin = await _context.Admins
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Name == name && a.Password == pass);

            if (admin == null)
            {
                return new RepoResponse<Admin>()
                {
                    Content = null,
                    IsSucces = false,
                    Message = "Yanlış məlumatlar daxil etmisiniz."
                };
            }

            var token = GenerateAdminToken(admin);
            return new RepoResponse<Admin>()
            {
                Content = admin,
                IsSucces = true,
                Message = $"Bearer {token}"
            };
        }

        public async Task<RepoResponse<Seller>> Login(SellerLoginDto seller)
        {
            var sellers = await _context.Sellers
                .AsNoTracking()
                .Where(s => s.Email == seller.Email)
                .ToListAsync();

            foreach (var s in sellers)
            {
                if (s.Password == seller.Password)
                {
                    var token = GenerateSellerToken(s);
                    return new RepoResponse<Seller>()
                    {
                        Content = s,
                        IsSucces = true,
                        Message = $"Bearer {token}"
                    };
                }
            }

            return new RepoResponse<Seller>()
            {
                Content = null,
                IsSucces = false,
                Message = "Daxil olunan məlumatlar yanlışdır."
            };
        }

        public async Task<RepoResponse<Seller>> Register(SellerRegisterDto seller)
        {
            var oldSeller = await _context.Sellers
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Email == seller.Email);
            if (oldSeller != null)
            {
                return new RepoResponse<Seller>()
                {
                    Content = null,
                    IsSucces = false,
                    Message = "Daxil edilmiş email ilə artıq satıcı mövcuddur."
                };
            }

            var newGuid = Guid.NewGuid();
            var newSeller = new Seller()
            {
                Id = newGuid,
                Name = seller.Name,
                Details = seller.Details,
                Email = seller.Email,
                Password = seller.Password
            };

            _context.Sellers.Add(newSeller);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                var returnValue = await Login(new SellerLoginDto()
                {
                    Email = newSeller.Email,
                    Password = seller.Password
                });
                if (returnValue.IsSucces)
                {
                    return new RepoResponse<Seller>()
                    {
                        Content = returnValue.Content,
                        IsSucces = true,
                        Message = returnValue.Message
                    };
                }
            }

            return new RepoResponse<Seller>()
            {
                Content = null,
                IsSucces = false,
                Message = "Verilənlər bazasına satıcını əlavə etmək mümkün olmadı."
            };
        }

        private string GenerateSellerToken(Seller sellerInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, sellerInfo.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Aud, _config["Jwt:Audience"]),
                new Claim(ClaimTypes.Role, "seller")
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }

        private string GenerateAdminToken(Admin adminInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, adminInfo.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Aud, _config["Jwt:Audience"]),
                new Claim(ClaimTypes.Role, "admin")
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }
    }
}
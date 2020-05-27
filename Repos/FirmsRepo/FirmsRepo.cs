using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarDMS.Data;
using StarDMS.Models;
using StarDMS.Models.Dtos;
using StarDMS.Utilities;

namespace StarDMS.Repos.FirmsRepo
{
    public class FirmsRepo : IFirmsRepo
    {
        private readonly StarDMSContext _context;

        public FirmsRepo(StarDMSContext context)
        {
            this._context = context;
        }

        public async Task<RepoResponse<FirmReturnDto>> AddFirm(FirmAddDto newFirm)
        {
            var oldFirm = await _context.Firms
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Name.ToLower() == newFirm.Name.ToLower());
            if (oldFirm != null)
            {
                return new RepoResponse<FirmReturnDto>()
                {
                    Content = new FirmReturnDto()
                    {
                        Id = oldFirm.Id,
                        Contact = oldFirm.Contact,
                        Details = oldFirm.Details,
                        Name = oldFirm.Name
                    },
                    IsSucces = true,
                    Message = "Bazada eyni adla istehsalçı mövcuddur."
                };
            }

            var nf = new Firm()
            {
                Id = Guid.NewGuid(),
                Contact = newFirm.Contact,
                Details = newFirm.Details,
                Name = newFirm.Name
            };

            await _context.Firms.AddAsync(nf);
            var res = await _context.SaveChangesAsync();

            if (res > 0)
            {
                return new RepoResponse<FirmReturnDto>()
                {
                    Content = await _context.Firms
                        .AsNoTracking()
                        .Select(f => new FirmReturnDto()
                        {
                            Id = f.Id,
                            Contact = f.Contact,
                            Details = f.Details,
                            Name = f.Name
                        })
                        .FirstOrDefaultAsync(f => f.Id == nf.Id),
                    IsSucces = true,
                    Message = "İstehsalçı bazaya müvəffəqiyyətlə əlavə olundu."
                };
            }

            return new RepoResponse<FirmReturnDto>()
            {
                Content = null,
                IsSucces = false,
                Message = "Baza ilə bağlı problem yarandı. İstehsalçı əlavə olunmadı."
            };
        }

        public async Task<RepoResponse<FirmReturnDto>> EditFirm(Firm editedFirm)
        {
            var oldF = await _context.Firms
                .FirstOrDefaultAsync(f => f.Id == editedFirm.Id);
            
            if (oldF == null)
            {
                return new RepoResponse<FirmReturnDto>()
                {
                    Content = null,
                    IsSucces = false,
                    Message = "Verilmiş İD ilə bazada istehsalçı tapılmadı."
                };
            }

            oldF.Name = editedFirm.Name;
            oldF.Contact = editedFirm.Contact;
            oldF.Details = editedFirm.Details;

            var res = await _context.SaveChangesAsync();
            if (res > 0)
            {
                return new RepoResponse<FirmReturnDto>()
                {
                    Content = await _context.Firms
                        .AsNoTracking()
                        .Select(f => new FirmReturnDto()
                        {
                            Id = f.Id,
                            Contact = f.Contact,
                            Details = f.Details,
                            Name = f.Name
                        })
                        .FirstOrDefaultAsync(f => f.Id == oldF.Id),
                    IsSucces = true,
                    Message = "İstehsalçı məlumatları müvəffəqiyyətlə yeniləndi."
                };
            }

            return new RepoResponse<FirmReturnDto>()
            {
                Content = null,
                IsSucces = false,
                Message = "Baza ilə bağlı problem yarandı. Məlumatları yeniləmək mümkün olmadı."
            };
        }

        public async Task<RepoResponse<List<FirmReturnDto>>> GetFirms()
        {
            var firms = await _context.Firms
                .AsNoTracking()
                .Select(f => new FirmReturnDto()
                {
                    Id = f.Id,
                    Contact = f.Contact,
                    Details = f.Details,
                    Name = f.Name
                })
                .OrderBy(f => f.Name)
                .ToListAsync();
            
            return new RepoResponse<List<FirmReturnDto>>()
            {
                Content = firms,
                IsSucces = true,
                Message = $"{firms.Count} istehsalçı tapıldı."
            };
        }

        public async Task<RepoResponse<List<FirmReturnBriefDto>>> GetFirmsBrief()
        {
            var firms = await _context.Firms
                .AsNoTracking()
                .Select(f => new FirmReturnBriefDto()
                {
                    Id = f.Id,
                    Name = f.Name
                })
                .OrderBy(f => f.Name)
                .ToListAsync();
            
            return new RepoResponse<List<FirmReturnBriefDto>>()
            {
                Content = firms,
                IsSucces = true,
                Message = $"{firms.Count} istehsalçı tapıldı."
            };
        }
    }
}
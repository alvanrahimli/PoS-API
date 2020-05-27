using System.Collections.Generic;
using System.Threading.Tasks;
using StarDMS.Models;
using StarDMS.Models.Dtos;
using StarDMS.Utilities;

namespace StarDMS.Repos.FirmsRepo
{
    public interface IFirmsRepo
    {
        Task<RepoResponse<List<FirmReturnDto>>> GetFirms();
        Task<RepoResponse<List<FirmReturnBriefDto>>> GetFirmsBrief();
        Task<RepoResponse<FirmReturnDto>> AddFirm(FirmAddDto newFirm);
        Task<RepoResponse<FirmReturnDto>> EditFirm(Firm editedFirm);
    }
}
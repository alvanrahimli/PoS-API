using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarDMS.Models;
using StarDMS.Models.Dtos;
using StarDMS.Repos.FirmsRepo;

namespace StarDMS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "admin")]
    public class FirmsController : ControllerBase
    {
        private readonly IFirmsRepo _repo;
        public FirmsController(IFirmsRepo repo)
        {
            this._repo = repo;
        }

        [HttpGet]
        [Route("AllBrief")]
        public async Task<IActionResult> GetAllBrief()
        {
            var response = await _repo.GetFirmsBrief();

            if (response.IsSucces)
            {
                return Ok(response.Content);
            }

            return NotFound(response.Message);
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _repo.GetFirms();

            if (response.IsSucces)
            {
                return Ok(response.Content);
            }

            return NotFound(response.Message);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddFirm(FirmAddDto newF)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Düzgün istehsalçı məlumatları əks olunmayıb.");
            }

            var response = await _repo.AddFirm(newF);
            if (response.IsSucces)
            {
                return Ok(response.Content);
            }

            return StatusCode(520, response.Message);
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> AddFirm(Firm edited)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Düzgün istehsalçı məlumatları əks olunmayıb.");
            }

            var response = await _repo.EditFirm(edited);
            if (response.IsSucces)
            {
                return Ok(response.Content);
            }

            return StatusCode(520, response.Message);
        }
    }
}
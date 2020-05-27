using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarDMS.Models.Dtos;
using StarDMS.Repos.Auth;

namespace StarDMS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo _repo;
        public AuthController(IAuthRepo repo)
        {
            this._repo = repo;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]SellerRegisterDto regCreds)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _repo.Register(regCreds);

            if (result.IsSucces)
            {
                var returnDto = new SellerReturnDto()
                {
                    Credentials = result.Content,
                    Token = result.Message
                };

                return Ok(returnDto);
            }

            return StatusCode(500, result.Message);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]SellerLoginDto loginCreds)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _repo.Login(loginCreds);

            if (result.IsSucces)
            {
                var returnDto = new SellerReturnDto()
                {
                    Credentials = result.Content,
                    Token = result.Message
                };

                return Ok(returnDto);
            }

            return Unauthorized(loginCreds);
        }

        [HttpPost]
        [Route("AdminLogin")]
        public async Task<IActionResult> AdminLogin([FromBody]AdminLoginDto loginCreds)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _repo.AdminLogin(loginCreds.Name, loginCreds.Password);

            if (result.IsSucces)
            {
                var returnDto = new AdminReturnDto()
                {
                    Credentials = result.Content,
                    Token = result.Message
                };

                return Ok(returnDto);
            }

            return Unauthorized(loginCreds);
        }
    
        [HttpGet]
        [Route("VerifySeller")]
        [Authorize(Roles = "seller")]
        public IActionResult VerifySeller() => Ok();

        [HttpGet]
        [Route("VerifyAdmin")]
        [Authorize(Roles = "admin")]
        public IActionResult VerifyAdmin() => Ok();
    }
}
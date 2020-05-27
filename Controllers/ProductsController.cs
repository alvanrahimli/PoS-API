using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarDMS.Models;
using StarDMS.Models.Dtos;
using StarDMS.Repos.Products;

namespace StarDMS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // [Authorize(Roles = "admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepo _repo;
        public ProductsController(IProductsRepo repo)
        {
            this._repo = repo;
        }

        [HttpGet]
        [Route("GetProduct")]
        [Authorize]
        public async Task<IActionResult> GetProduct(string barcode)
        {
            if (barcode.Length != 13)
                return BadRequest("Barkod 13 simvol olmalıdır.");
            
            var result = await _repo.GetProduct(barcode);
            if (result.IsSucces)
            {
                return Ok(result.Content);
            }

            return StatusCode(520, result.Message);
        }

        [HttpGet]
        [Route("GetProducts")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetProducts(int rq, int c)
        {
            if (rq * c < 0)
                return BadRequest("Mənfi sorğu nömrəsi və ya səhifəlik məhsul sayı daxil edilib.");
            
            var result = await _repo.GetProducts(rq, c);
            if (result.IsSucces)
            {
                return Ok(result.Content);
            }

            return StatusCode(520, result.Message);
        }

        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddProduct([FromBody]ProductAddDto newP)
        {
            if (!ModelState.IsValid)
                return BadRequest("Səhv özəlliklər daxil edilib." + ModelState);

            var result = await _repo.AddProduct(newP);
            if (result.IsSucces)
            {
                return Ok(result.Content);
            }

            return StatusCode(520, result.Message);
        }

        [HttpPost]
        [Route("AddMultiple")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddMultiple([FromBody]List<ProductAddDto> newPs)
        {
            if (!ModelState.IsValid)
                return BadRequest("Səhv özəlliklər daxil edilib.");
            
            var result = await _repo.AddMultipleProducts(newPs);
            if (result.IsSucces)
            {
                return Ok(result.Content);
            }

            return StatusCode(520, result.Message);
        }

        [HttpPost]
        [Route("Update")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([FromBody]Product p)
        {
            if (!ModelState.IsValid)
                return BadRequest("Səhv özəlliklər daxil edilib.");
            
            var result = await _repo.UpdateProduct(p);
            if (result.IsSucces)
            {
                return Ok(result.Content);
            }

            return StatusCode(520, result.Message);
        }

        [HttpPost]
        [Route("Delete")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string barcode)
        {
            if (barcode.Length != 13)
                return BadRequest("Barkod 13 simvol olmalıdır.");
            
            var result = await _repo.DeleteProduct(barcode);
            if (result.IsSucces)
            {
                return Ok(result.Content);
            }

            return StatusCode(520, result.Message);
        }
    
        [HttpGet]
        [Route("GetEmptyStocks")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetEmptyStocks()
        {
            var result = await _repo.GetEmptyStocks();
            if (result.IsSucces)
            {
                return Ok(result.Content);
            }

            return StatusCode(520, result.Message);
        }
    }
}
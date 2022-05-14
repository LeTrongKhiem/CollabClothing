using CollabClothing.Application.Catalog.Banners;
using CollabClothing.ViewModels.Catalog.Banners;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannersController : ControllerBase
    {
        private readonly IBannerService _bannerService;
        public BannersController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BannerCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var bannerId = await _bannerService.Create(request);
            if (bannerId.Equals("") || bannerId == null)
            {
                return BadRequest();
            }
            var banner = await _bannerService.GetBannerById(bannerId);
            return CreatedAtAction(nameof(bannerId), new { id = bannerId }, banner);
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetById(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var banner = await _bannerService.GetBannerById(id);
            if (banner == null)
            {
                return BadRequest();
            }
            return Ok(banner);
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAll([FromQuery] PagingWithKeyword request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _bannerService.GetAllPaging(request);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _bannerService.Delete(id);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

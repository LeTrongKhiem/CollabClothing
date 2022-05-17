using CollabClothing.Application.Catalog.Brands;
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
    public class BrandsController : ControllerBase
    {
        private IBrandService _brandService;
        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAll([FromQuery] PagingWithKeyword request)
        {
            var brand = await _brandService.GetAllPaging(request);
            return Ok(brand);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBrand()
        {
            var result = await _brandService.GetAll();
            return Ok(result);
        }
    }
}

using CollabClothing.Application.Catalog.Brands;
using CollabClothing.ViewModels.Catalog.Brands;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] PagingWithKeyword request)
        {
            var brand = await _brandService.GetAllPaging(request);
            return Ok(brand);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllBrand()
        {
            var result = await _brandService.GetAll();
            return Ok(result);
        }
        [HttpPost]
        [AllowAnonymous]
        [Consumes("mutilpart/form-data")]
        public async Task<IActionResult> Create([FromForm] BrandCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _brandService.Create(request);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _brandService.Delete(id);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut("{id}")]
        [AllowAnonymous]
        [Consumes("mutilpart/form-data")]
        public async Task<IActionResult> Edit(string id, [FromForm] BrandEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _brandService.Edit(id, request);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _brandService.GetByBrandId(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

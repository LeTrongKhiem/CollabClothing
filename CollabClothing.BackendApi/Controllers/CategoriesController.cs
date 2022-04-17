using CollabClothing.Application.Catalog.Categories;
using CollabClothing.ViewModels.Catalog.Categories;
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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet("paging")]
        //[AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] GetCategoryRequestPaging request)
        {
            var cate = await _categoryService.GetAllPaging(request);
            return Ok(cate);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var cate = await _categoryService.GetCateById(Id);
            if (cate == null)
            {
                return BadRequest();
            }
            return Ok(cate);
        }
        [HttpPost]
        //[AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var cateId = _categoryService.Create(request);
            if (cateId.Equals("") || cateId == null)
            {
                return BadRequest();
            }
            var cate = await _categoryService.GetCateById(await cateId);
            return CreatedAtAction(nameof(cateId), new { id = cateId }, cate);

        }
    }
}

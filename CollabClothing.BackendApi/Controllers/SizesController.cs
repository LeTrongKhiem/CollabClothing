using CollabClothing.Application.Catalog.Sizes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizesController : ControllerBase
    {
        private readonly ISizeService _sizeService;
        public SizesController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _sizeService.GetAllSize();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNameSize(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _sizeService.GetNameSize(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

using CollabClothing.Application.Catalog.Color;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly IColorService _colorService;
        public ColorsController(IColorService colorService)
        {
            _colorService = colorService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _colorService.GetAllColor();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNameColor(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _colorService.GetNameColor(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

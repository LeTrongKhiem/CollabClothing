using CollabClothing.Application.Catalog.Promotions;
using CollabClothing.ViewModels.Catalog.Promotions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.BackendApi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        public PromotionsController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _promotionService.GetAll();

            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("paging")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] PromotionPaging request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _promotionService.GetAllPaging(request);

            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("promotion/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPromotionByProductId(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _promotionService.GetPromotionByProductId(id);

            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] PromotionCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _promotionService.Create(request);

            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(string id, [FromBody] PromotionEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _promotionService.Edit(id, request);

            if (result)
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
                return BadRequest(ModelState);
            }
            var result = await _promotionService.Delete(id);

            if (result)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

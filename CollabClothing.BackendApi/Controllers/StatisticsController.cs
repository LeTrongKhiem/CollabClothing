﻿using CollabClothing.Application.Catalog.Statistic;
using CollabClothing.ViewModels.Catalog.Statistic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CollabClothing.BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IBenefitService _benefitService;
        public StatisticsController(IBenefitService benefitService)
        {
            _benefitService = benefitService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllByDay([FromQuery] BenefitRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _benefitService.GetAllByDay(request);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] BenefitRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _benefitService.GetAll(request);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}

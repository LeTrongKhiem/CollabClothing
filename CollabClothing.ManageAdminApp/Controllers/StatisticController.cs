using CollabClothing.ApiShared;
using CollabClothing.ViewModels.Catalog.Statistic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Controllers
{
    public class StatisticController : BaseController
    {
        private readonly IStatisticApiClient _statisticApiClient;
        public StatisticController(IStatisticApiClient statisticApiClient)
        {
            _statisticApiClient = statisticApiClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        //[HttpGet]
        //public IActionResult GetByDay()
        //{
        //    return View();
        //}
        [HttpGet]
        public async Task<IActionResult> GetByDay(DateTime fromDate, DateTime toDate)
        {
            var request = new BenefitRequest()
            {
                FromDate = fromDate,
                ToDate = toDate
            };
            var result = await _statisticApiClient.GetAllByDay(request);
            if (result.Count != 0)
            {
                return Ok(result);
            }
            return View();
        }
    }
}

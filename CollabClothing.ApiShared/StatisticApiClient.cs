using CollabClothing.ViewModels.Catalog.Statistic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public class StatisticApiClient : BaseApiClient, IStatisticApiClient
    {
        public StatisticApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<List<BenefitViewModel>> GetAllByDay(BenefitRequest request)
        {
            return await GetListAsync<BenefitViewModel>($"/api/statistics?fromdate={request.FromDate}&todate={request.ToDate}");
        }
    }
}

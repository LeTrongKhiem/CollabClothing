using CollabClothing.ViewModels.Catalog.Statistic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public class StatisticApiClient : BaseApiClient, IStatisticApiClient
    {
        public StatisticApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<List<OutputStatisticTMDT>> GetAll(BenefitRequest request)
        {
            return await GetListAsync<OutputStatisticTMDT>($"/api/statistics/all?fromdate={request.FromDate}&todate={request.ToDate}");
        }

        public async Task<List<BenefitViewModel>> GetAllByDay(BenefitRequest request)
        {
            return await GetListAsync<BenefitViewModel>($"/api/statistics?fromdate={request.FromDate}&todate={request.ToDate}");
        }
    }
}

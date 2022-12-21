using CollabClothing.ViewModels.Catalog.Statistic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public interface IStatisticApiClient
    {
        Task<List<BenefitViewModel>> GetAllByDay(BenefitRequest request);
        Task<List<OutputStatisticTMDT>> GetAll(BenefitRequest request);
    }
}

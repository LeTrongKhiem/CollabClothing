using CollabClothing.ViewModels.Catalog.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public interface IStatisticApiClient
    {
        Task<List<BenefitViewModel>> GetAllByDay(BenefitRequest request);
    }
}

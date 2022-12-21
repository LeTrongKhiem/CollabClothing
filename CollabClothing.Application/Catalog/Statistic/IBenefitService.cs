using CollabClothing.Data.Entities;
using CollabClothing.ViewModels.Catalog.Statistic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Statistic
{
    public interface IBenefitService
    {
        public Task<List<OutputStatistic>> GetAllByDay(BenefitRequest request);
        public Task<List<Data.Entities.OutputStatisticTMDT>> GetAll(BenefitRequest request);
    }
}

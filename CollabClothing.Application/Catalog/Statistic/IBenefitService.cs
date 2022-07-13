using CollabClothing.Data.Entities;
using CollabClothing.ViewModels.Catalog.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Statistic
{
    public interface IBenefitService
    {
        public Task<List<OutputStatistic>> GetAllByDay(BenefitRequest request);
    }
}

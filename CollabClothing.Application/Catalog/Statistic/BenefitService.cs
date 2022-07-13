using CollabClothing.Data.EF;
using CollabClothing.Data.Entities;
using CollabClothing.ViewModels.Catalog.Statistic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Statistic
{
    public class BenefitService : IBenefitService
    {
        private readonly CollabClothingDBContext _context;
        public BenefitService(CollabClothingDBContext context)
        {
            _context = context;
        }
        public async Task<List<OutputStatistic>> GetAllByDay(BenefitRequest request)
        {
            String sql = "exec BenefitStatistic @fromdate='" + request.FromDate + "', @todate='" + request.ToDate + "'";
            return await _context.OutputStatistics.FromSqlRaw(sql).ToListAsync();
        }

    }
}

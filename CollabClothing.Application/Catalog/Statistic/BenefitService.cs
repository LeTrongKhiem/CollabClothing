using CollabClothing.Data.EF;
using CollabClothing.Data.Entities;
using CollabClothing.ViewModels.Catalog.Statistic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<List<Data.Entities.OutputStatisticTMDT>> GetAll(BenefitRequest request)
        {
            String sql = "exec BenefitStatistic1 @fromdate='" + request.FromDate + "', @todate='" + request.ToDate + "'";
            return await _context.OutputStatisticTMDTs.FromSqlRaw(sql).ToListAsync();
        }

        public async Task<List<OutputStatistic>> GetAllByDay(BenefitRequest request)
        {
            String sql = "exec BenefitStatistic @fromdate='" + request.FromDate + "', @todate='" + request.ToDate + "'";
            return await _context.OutputStatistics.FromSqlRaw(sql).ToListAsync();
        }



    }
}

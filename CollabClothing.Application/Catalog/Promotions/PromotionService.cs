using CollabClothing.Application.Common;
using CollabClothing.Data.EF;
using CollabClothing.Data.Entities;
using CollabClothing.Utilities.Exceptions;
using CollabClothing.ViewModels.Catalog.Promotions;
using CollabClothing.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Promotions
{
    public class PromotionService : IPromotionService
    {
        private readonly CollabClothingDBContext _context;
        public PromotionService(CollabClothingDBContext context)
        {
            _context = context;
        }
        public async Task<string> Create(PromotionCreateRequest request)
        {
            var g = Guid.NewGuid();
            var promotion = new PromotionDetail()
            {
                Id = g.ToString(),
                Info = request.Info,
                More = request.More,
                OnlinePromotion = request.Online,
                NamePromotion = request.NamePromotion
            };
            _context.PromotionDetails.Add(promotion);
            await _context.SaveChangesAsync();
            return promotion.Id;
        }

        public async Task<bool> Delete(string id)
        {
            var promotion = await _context.PromotionDetails.FindAsync(id);
            if (promotion == null)
            {
                throw new CollabException("Not found");
            }
            _context.PromotionDetails.Remove(promotion);
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<bool> Edit(string id, PromotionEditRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PromotionViewModel>> GetAll()
        {
            var query = from promotion in _context.PromotionDetails select promotion;
            var result = await query.Select(x => new PromotionViewModel()
            {
                Info = x.Info,
                More = x.More,
                NamePromotion = x.NamePromotion,
                Online = x.OnlinePromotion,
            }).ToListAsync();
            return result;
        }

        public Task<PageResult<PromotionViewModel>> GetAllPaging(PagingWithKeyword request)
        {
            throw new NotImplementedException();
        }

        public Task<PromotionViewModel> GetBannerById(string id)
        {
            throw new NotImplementedException();
        }
    }
}

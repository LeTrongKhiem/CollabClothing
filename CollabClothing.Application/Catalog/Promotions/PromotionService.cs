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
            var promotionMap = await _context.Promotions.Where(x => x.Id == id).ToListAsync();
            var promotion = await _context.PromotionDetails.FindAsync(id);
            if (promotion == null)
            {
                throw new CollabException("Not found");
            }
            foreach (var item in promotionMap)
            {
                _context.Promotions.Remove(item);
            }
            _context.PromotionDetails.Remove(promotion);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Edit(string id, PromotionEditRequest request)
        {
            var promotion = await _context.PromotionDetails.FindAsync(id);
            if (promotion == null)
            {
                throw new CollabException("Not found promotion");
            }
            promotion.NamePromotion = request.NamePromotion ?? promotion.NamePromotion;
            promotion.More = request.More;
            promotion.OnlinePromotion = request.Online;
            promotion.Info = request.Info ?? promotion.Info;
            _context.PromotionDetails.Update(promotion);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<List<PromotionViewModel>> GetAll()
        {
            var query = from promotion in _context.PromotionDetails select promotion;
            var result = await query.Select(x => new PromotionViewModel()
            {
                Id = x.Id,
                Info = x.Info,
                More = x.More,
                NamePromotion = x.NamePromotion,
                Online = x.OnlinePromotion,
            }).ToListAsync();
            return result;
        }

        public async Task<PageResult<PromotionViewModel>> GetAllPaging(PromotionPaging request)
        {
            var query = from promotion in _context.PromotionDetails select promotion;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.NamePromotion.Contains(request.Keyword));
            }
            if (request.Online == true)
            {
                query = query.Where(x => x.OnlinePromotion == true);
            }
            if (request.More == true)
            {
                query = query.Where(x => x.More == true);
            }
            int countPage = await _context.PromotionDetails.CountAsync();
            var items = await query.Skip((request.PageIndex - 1) * request.PageSize)
                            .Take(request.PageSize)
                            .Select(x => new PromotionViewModel()
                            {
                                Id = x.Id,
                                Info = x.Info,
                                More = x.More,
                                NamePromotion = x.NamePromotion,
                                Online = x.OnlinePromotion
                            }).ToListAsync();
            var pageResult = new PageResult<PromotionViewModel>()
            {
                Items = items,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecord = countPage
            };
            return pageResult;
        }

        public async Task<List<PromotionViewModel>> GetPromotionByProductId(string productId)
        {
            var query = from p in _context.PromotionDetails
                        join pmp in _context.Promotions
                        on p.Id equals pmp.Id
                        into pmpp
                        from pmp in pmpp.DefaultIfEmpty()
                        where pmp.ProductId == productId
                        select new { p, pmp };
            var data = await query.Select(x => new PromotionViewModel()
            {
                Id = x.p.Id,
                Info = x.p.Info,
                More = x.p.More,
                NamePromotion = x.p.NamePromotion,
                Online = x.p.OnlinePromotion
            }).ToListAsync();
            return data;
        }
    }
}

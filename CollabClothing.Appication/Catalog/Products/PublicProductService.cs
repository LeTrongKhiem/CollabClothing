
using CollabClothing.WebApp.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using CollabClothing.ViewModels.Common;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.Appication.Catalog.Products.Dtos.Public;

namespace CollabClothing.Appication.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly DBContext _context;
        public PublicProductService(DBContext context)
        {
            _context = context;
        }
        public async Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetRequestPagingProduct request)
        {
            int CategoryIdInt = Convert.ToInt32(request.CategoryId);

            var query = from p in _context.Products
                        join pmc in _context.ProductMapCategories on p.Id equals pmc.ProductId
                        join c in _context.Categories on pmc.CategoryId equals c.Id
                        select new { p, pmc };
            if (request.CategoryId != null && !request.CategoryId.Equals("0"))
            {
                query = query.Where(p => p.pmc.CategoryId == request.CategoryId);
            }
            //Paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .Select(x => new ProductViewModel()
                                    {
                                        Id = x.p.Id,
                                        ProductName = x.p.ProductName,
                                        BrandId = x.p.BrandId,
                                        Description = x.p.Description,
                                        Installment = x.p.Installment,
                                        PriceCurrent = x.p.PriceCurrent,
                                        PriceOld = x.p.PriceOld,
                                        SaleOff = x.p.SaleOff,
                                        Slug = x.p.Slug,
                                        SoldOut = x.p.SoldOut
                                    })
                .ToListAsync();
            var pagedResult = new PageResult<ProductViewModel>()
            {
                Items = data,
                TotalRecord = totalRow
            };
            return pagedResult;
        }
    }
}

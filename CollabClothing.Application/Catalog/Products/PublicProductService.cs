
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using CollabClothing.ViewModels.Common;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.Data.EF;

namespace CollabClothing.Application.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly CollabClothingDBContext _context;
        public PublicProductService(CollabClothingDBContext context)
        {
            _context = context;
        }


        public async Task<List<ProductViewModel>> GetAll()
        {
            var data = await (from p in _context.Products
                              join pmc in _context.ProductMapCategories on p.Id equals pmc.ProductId into ppmc
                              from pmc in ppmc.DefaultIfEmpty()
                              join c in _context.Categories on pmc.CategoryId equals c.Id into pmcc
                              from c in pmcc.DefaultIfEmpty()
                              join pimg in _context.ProductImages on p.Id equals pimg.ProductId into ppimg
                              from pimg in ppimg.DefaultIfEmpty()
                              select new ProductViewModel()
                              {
                                  Id = p.Id,
                                  ProductName = p.ProductName,
                                  BrandId = p.BrandId,
                                  Description = p.Description,
                                  Installment = p.Installment,
                                  PriceCurrent = p.PriceCurrent,
                                  PriceOld = p.PriceOld,
                                  SaleOff = p.SaleOff,
                                  Slug = p.Slug,
                                  SoldOut = p.SoldOut,
                                  CategoryName = c.NameCategory,
                                  ThumbnailImage = pimg.Path != null ? pimg.Path : "no-image in product"
                              }).ToListAsync();
            return data;
        }

        public async Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductRequestPaging request)
        {
            var query = from p in _context.Products
                        join pmc in _context.ProductMapCategories on p.Id equals pmc.ProductId into ppmc
                        from pmc in ppmc.DefaultIfEmpty()
                        join c in _context.Categories on pmc.CategoryId equals c.Id into pmcc
                        from c in pmcc.DefaultIfEmpty()
                        join pimg in _context.ProductImages on p.Id equals pimg.ProductId into ppimg
                        from pimg in ppimg.DefaultIfEmpty()
                        select new { p, pmc, c, pimg };
            if (request.CategoryId != null && !request.CategoryId.Equals("0"))
            {
                query = query.Where(p => p.pmc.CategoryId == request.CategoryId);
                // image = image.Where(x => x.pd.prod);
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
                                        SoldOut = x.p.SoldOut,
                                        // ThumbnailImage = image
                                        CategoryName = x.c.NameCategory,
                                        ThumbnailImage = x.pimg.Path
                                    })
                .ToListAsync();
            var pagedResult = new PageResult<ProductViewModel>()
            {
                Items = data,
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            return pagedResult;
        }
    }
}

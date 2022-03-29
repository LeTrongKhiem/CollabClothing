
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using CollabClothing.ViewModels.Common;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.WebApp.Models;

namespace CollabClothing.Appication.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly DBClothingContext _context;
        public PublicProductService(DBClothingContext context)
        {
            _context = context;
        }


        public async Task<List<ProductViewModel>> GetAll()
        {
            // var query = from p in _context.Products
            //             join pmc in _context.ProductMapCategories on p.Id equals pmc.ProductId
            //             join c in _context.Categories on pmc.CategoryId equals c.Id
            //             // into ProductInfo
            //             // from productData in ProductInfo.DefaultIfEmpty()
            //             // join pimg in _context.ProductImages on p.Id equals pimg.ProductId
            //             select new { p, pmc };

            // var data = await query.Select(x => new ProductViewModel()
            // {
            //     Id = x.p.Id,
            //     ProductName = x.p.ProductName,
            //     BrandId = x.p.BrandId,
            //     Description = x.p.Description,
            //     Installment = x.p.Installment,
            //     PriceCurrent = x.p.PriceCurrent,
            //     PriceOld = x.p.PriceOld,
            //     SaleOff = x.p.SaleOff,
            //     Slug = x.p.Slug,
            //     SoldOut = x.p.SoldOut,
            //     // ThumbnailImage = x.pimg.Path != null ? x.pimg.Path : "no-image in product"
            // }).ToListAsync();
            var data = await (from p in _context.Products
                              join pimg in _context.ProductImages on new { PID = p.Id } equals new { PID = pimg.ProductId } into ProductInfoRight
                              from productsInfoRightData in ProductInfoRight.DefaultIfEmpty()
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
                                  ThumbnailImage = productsInfoRightData.Path != null ? productsInfoRightData.Path : "no-image in product"
                              }).ToListAsync();
            // var allData = data.Union(data2).ToList();
            return data;
        }

        public async Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductRequestPaging request)
        {
            var query = from p in _context.Products
                        join pmc in _context.ProductMapCategories on p.Id equals pmc.ProductId
                        join c in _context.Categories on pmc.CategoryId equals c.Id
                        select new { p, pmc, c };
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

using CollabClothing.Appication.Catalog.Products.Dtos;
using CollabClothing.Appication.Catalog.Products.Dtos.Manage;
using CollabClothing.Appication.Dtos;
using CollabClothing.Utilities.Exceptions;
using CollabClothing.WebApp.Data;
using CollabClothing.WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Appication.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly DBContext _context;
        public ManageProductService(DBContext context)
        {
            _context = context;
        }

        public async Task AddViewCount(string productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Id = request.Id,
                ProductName = request.ProductName,
                PriceCurrent = request.PriceCurrent,
                PriceOld = request.PriceOld,
                SaleOff = request.SaleOff,
                BrandId = request.BrandId,
                SoldOut = request.SoldOut,
                Installment = request.Installment,
                Description = request.Description,
                Slug = request.Slug,
                ViewCount = 0,
                ProductMapCategories = new List<ProductMapCategory>()
                {
                    new ProductMapCategory()
                    {
                        CategoryId = request.Category.NameCategory
                    }
                }
            };
            _context.Products.Add(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(string productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (productId == null)
                throw new CollabException($"Cannot find a product: {productId}");
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ProductViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<PageResult<ProductViewModel>> GetAllPaging(GetRequestPagingProduct request)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pmc in _context.ProductMapCategories on p.Id equals pmc.ProductId
                        join c in _context.Categories on pmc.CategoryId equals c.Id
                        select new { p, pmc };
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.p.ProductName.Contains(request.Keyword));
            }
            if (request.CategoryIds.Count > 0)
            {
                query = query.Where(p => request.CategoryIds.Contains(p.pmc.CategoryId));
            }
            //3. paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).Select(x => new ProductViewModel()
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

            //4. select and projection

            var pagedResult = new PageResult<ProductViewModel>()
            {
                Items = data,
                TotalRecord = totalRow
            };

        }

        public async Task<int> Update(ProductEditRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateSaleOff(int productId, int newSaleOff)
        {
            throw new NotImplementedException();
        }
    }
}

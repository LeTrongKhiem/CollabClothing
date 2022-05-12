using CollabClothing.Application.Common;
using CollabClothing.Data.Dtos;
using CollabClothing.Data.EF;
using CollabClothing.Data.Entities;
using CollabClothing.Data.Extensions;
using CollabClothing.ViewModels.Catalog.Brands;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Brands
{
    public class BrandService : IBrandService
    {
        private readonly CollabClothingDBContext _context;
        private readonly IStorageService _storageService;
        private readonly string USER_CONTENT_FOLDER_NAME = "user-content";
        private readonly string CHILD_PATH_FOLDER_NAME = "img-brand";
        public BrandService(CollabClothingDBContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName, CHILD_PATH_FOLDER_NAME);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + CHILD_PATH_FOLDER_NAME + "/" + fileName;
        }
        public async Task<string> Create(BrandCreateRequest request)
        {
            Guid g = Guid.NewGuid();
            var brandDTO = new BrandDTO()
            {
                Id = g.ToString(),
                Info = request.Info,
                NameBrand = request.NameBrand,
                Slug = request.Slug
            };
            if (request.Images != null)
            {
                brandDTO.Images = await _storageService.SaveFile(request.Images, CHILD_PATH_FOLDER_NAME);
            }
            else
            {
                brandDTO.Images = "no-image";
            }
            Brand brand = new Brand();
            brand.BrandMapping(brandDTO);
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return brandDTO.Id;
        }

        public async Task<bool> Delete(string brandId)
        {
            var brand = await _context.Brands.FindAsync(brandId);
            if (brand == null)
            {
                return false;
            }
            var fullPath = "wwwroot" + brand.Images;
            if (brand.Images.Equals("no-image"))
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (File.Exists(fullPath))
                {
                    await Task.Run(() =>
                    {
                        File.Delete(fullPath);
                    });
                }
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public Task<bool> Edit(BrandEditRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<PageResult<BrandViewModel>> GetAllPaging(PagingWithKeyword request)
        {
            var query = from b in _context.Brands select new { b };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.b.NameBrand.Contains(request.Keyword));
            }
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .Select(x => new BrandViewModel()
                                    {
                                        Id = x.b.Id,
                                        Images = x.b.Images,
                                        Info = x.b.Info,
                                        NameBrand = x.b.NameBrand,
                                        Slug = x.b.Slug
                                    }).ToListAsync();
            var pageResult = new PageResult<BrandViewModel>()
            {
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pageResult;
        }

        public Task<BrandViewModel> GetByBrandId(string brandId)
        {
            throw new NotImplementedException();
        }
    }
}

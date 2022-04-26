using CollabClothing.Application.Common;
using CollabClothing.Data.Dtos;
using CollabClothing.Data.EF;
using CollabClothing.Data.Entities;
using CollabClothing.Data.Extensions;
using CollabClothing.ViewModels.Catalog.Brands;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Brand
{
    public class BrandService : IBrandService
    {
        private readonly CollabClothingDBContext _context;
        private readonly IStorageService _storageService;
        private readonly string USER_CONTENT_FOLDER_NAME = "user-content";
        private readonly string CHILD_PATH_FOLDER_NAME = "brand-image";
        public BrandService(CollabClothingDBContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public async Task<ResultApi<bool>> Create(BrandCreateRequest request)
        {
            Guid g = Guid.NewGuid();
            var brandDTO = new BrandDTO()
            {
                Id = g.ToString(),
                NameBrand = request.NameBrand,
                Info = request.Info,
                Slug = request.Slug
            };
            if (request.Images != null)
            {
                brandDTO.Images = await _storageService.SaveFile(request.Images, CHILD_PATH_FOLDER_NAME);
            }
            else
            {
                brandDTO.Images = "no image";
            }
            var brand = new Data.Entities.Brand();
            brand.BrandMapping(brandDTO);
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return new ResultApiSuccessed<bool>();
        }

        public async Task<ResultApi<bool>> Delete(string brandId)
        {
            var brand = await _context.Brands.FindAsync(brandId);
            if (brand == null)
            {
                return new ResultApiError<bool>("Không tồn tại Brand");
            }
            var fullPath = "wwwroot" + brand.Images;
            if (File.Exists(fullPath))
            {
                await Task.Run(() =>
                {
                    File.Delete(fullPath);
                });
            }
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return new ResultApiSuccessed<bool>();
        }

        public async Task<ResultApi<bool>> Edit(BrandEditRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultApi<PageResult<BrandViewModel>>> GetAllPaging(PagingWithKeyword request)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultApi<BrandViewModel>> GetByBrandId(string brandId)
        {
            throw new NotImplementedException();
        }
    }
}

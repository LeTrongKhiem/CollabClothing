using CollabClothing.Application.Common;
using CollabClothing.Data.Dtos;
using CollabClothing.Data.EF;
using CollabClothing.Data.Entities;
using CollabClothing.Data.Extensions;
using CollabClothing.ViewModels.Catalog.Banners;
using CollabClothing.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Banners
{
    public class BannerSevice : IBannerService
    {
        private readonly CollabClothingDBContext _context;
        private readonly IStorageService _storageService;
        private readonly string USER_CONTENT_FOLDER_NAME = "user-content";
        private readonly string CHILD_PATH_FLER_NAME = "img-banner";
        public BannerSevice(CollabClothingDBContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public async Task<string> Create(BannerCreateRequest request)
        {
            Guid g = Guid.NewGuid();
            var bannerDTO = new BannerDTO()
            {
                Id = g.ToString(),
                Alt = request.Alt,
                NameBanner = request.NameBanner,
                Text = request.Text,
                TypeBannerId = request.TypeBannerId,
            };
            if (request.Images != null)
            {
                bannerDTO.Images = await _storageService.SaveFile(request.Images, CHILD_PATH_FLER_NAME);
            }
            Banner banner = new Banner();
            banner.BannerMapping(bannerDTO);
            _context.Banners.Add(banner);
            await _context.SaveChangesAsync();
            return bannerDTO.Id;
        }

        public async Task<bool> Delete(string id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                return false;
            }
            var fullPath = "wwwroot" + banner.Images;
            if (File.Exists(fullPath))
            {
                await Task.Run(() =>
                {
                    File.Delete(fullPath);
                });
            }
            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> Edit(string id, BannerEditRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<PageResult<BannerViewModel>> GetAllPaging(PagingWithKeyword request)
        {
            var query = from bannerType in _context.BannerTypes
                        join banner in _context.Banners
                        on bannerType.Id equals banner.TypeBannerId
                        into bannerMapping
                        from banner in bannerMapping.DefaultIfEmpty()
                        select new { bannerType, banner };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.banner.NameBanner.Contains(request.Keyword));
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                                .Take(request.PageSize)
                                .Select(x => new BannerViewModel()
                                {
                                    Id = x.banner.Id,
                                    Alt = x.banner.Alt,
                                    Images = x.banner.Images,
                                    NameBanner = x.banner.NameBanner,
                                    Text = x.banner.Text,
                                    TypeBannerId = x.banner.TypeBannerId
                                }).ToListAsync();
            var pageResult = new PageResult<BannerViewModel>()
            {
                Items = data,
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            return pageResult;
        }

        public async Task<BannerViewModel> GetBannerById(string id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                return null;
            }
            var bannerVm = new BannerViewModel()
            {
                Id = id,
                Alt = banner.Alt,
                Images = banner.Images,
                NameBanner = banner.NameBanner,
                Text = banner.Text,
                TypeBannerId = banner.TypeBannerId
            };
            return bannerVm;
        }
    }
}

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
        private readonly string CHILD_PATH_FILE_NAME = "img-banner";
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
                bannerDTO.Images = await _storageService.SaveFile(request.Images, CHILD_PATH_FILE_NAME);
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

        public async Task<bool> Edit(string id, BannerEditRequest request)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                return false;
            }
            banner.Alt = request.Alt;
            banner.NameBanner = request.NameBanner;
            banner.TypeBannerId = request.TypeBannerId;
            banner.Text = request.Text;
            if (banner.Images == null)
            {
                if (request.Images != null)
                {
                    banner.Images = await _storageService.SaveFile(request.Images, CHILD_PATH_FILE_NAME);
                }
                _context.Banners.Add(banner);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (banner.Images != null)
                {
                    var fullPath = "wwwroot" + banner.Images;
                    await Task.Run(() =>
                    {
                        File.Delete(fullPath);
                    });
                    banner.Images = await _storageService.SaveFile(request.Images, CHILD_PATH_FILE_NAME);
                    _context.Banners.Update(banner);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    _context.Banners.Update(banner);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
        }

        public async Task<List<BannerViewModel>> GetAll()
        {
            var banner = await _context.Banners
                                  .OrderBy(x => x.Id)
                                  .Select(x => new BannerViewModel()
                                  {
                                      Id = x.Id,
                                      Alt = x.Alt,
                                      Images = x.Images,
                                      NameBanner = x.NameBanner,
                                      Text = x.Text,
                                      TypeBannerId = x.TypeBannerId
                                  }).ToListAsync();
            var banner1 = from b in _context.Banners
                          join bt in _context.BannerTypes
                            on b.TypeBannerId equals bt.Id into bbt
                          from bt in bbt.DefaultIfEmpty()
                          select new { b, bt };
            var result = await banner1.OrderBy(x => x.b.Id).Where(x => x.bt.NameBannerType.Equals("Khuyến mãi thời trang")).Select(x => new BannerViewModel()
            {
                Id = x.b.Id,
                Alt = x.b.Alt,
                Images = x.b.Images,
                NameBanner = x.b.NameBanner,
                Text = x.b.Text,
                TypeBannerId = x.b.TypeBannerId
            }).ToListAsync();
            return result;

        }

        public async Task<PageResult<BannerViewModel>> GetAllPaging(PagingWithKeyword request)
        {
            //var query = from bannerType in _context.BannerTypes
            //            join banner in _context.Banners
            //            on bannerType.Id equals banner.TypeBannerId
            //            into bannerMapping
            //            from banner in bannerMapping.DefaultIfEmpty()
            //            select new { bannerType, banner };
            var query = from banner in _context.Banners select new { banner };
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

        public async Task<List<BannerTypeViewModel>> GetAllType()
        {
            var banner = from b in _context.BannerTypes select new { b };
            return await banner.Select(x => new BannerTypeViewModel()
            {
                Id = x.b.Id,
                NameBannerType = x.b.NameBannerType
            }).ToListAsync();
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
                TypeBannerId = banner.TypeBannerId,

            };
            return bannerVm;
        }
    }
}

using CollabClothing.Application.Common;
using CollabClothing.Data.EF;
using CollabClothing.ViewModels.Catalog.Promotions;
using CollabClothing.ViewModels.Common;
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
        private readonly IStorageService _storageService;
        private readonly string USER_CONTENT_FOLDER_NAME = "user-content";
        public PromotionService(CollabClothingDBContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public Task<string> Create(PromotionCreateRequest request)
        {
            //Guid g = Guid.NewGuid();
            //var bannerDTO = new BannerDTO()
            //{
            //    Id = g.ToString(),
            //    Alt = request.Alt,
            //    NameBanner = request.NameBanner,
            //    Text = request.Text,
            //    TypeBannerId = request.TypeBannerId,
            //};
            //if (request.Images != null)
            //{
            //    bannerDTO.Images = await _storageService.SaveFile(request.Images, CHILD_PATH_FILE_NAME);
            //}
            //Banner banner = new Banner();
            //banner.BannerMapping(bannerDTO);
            //_context.Banners.Add(banner);
            //await _context.SaveChangesAsync();
            //return bannerDTO.Id;
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Edit(string id, PromotionEditRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<List<PromotionViewModel>> GetAll()
        {
            throw new NotImplementedException();
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

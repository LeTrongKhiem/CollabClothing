using CollabClothing.ViewModels.Catalog.Banners;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public interface IBannerApiClient
    {
        Task<bool> Create(BannerCreateRequest request);
        Task<bool> Delete(string id);
        Task<bool> Edit(string id, BannerEditRequest request);
        Task<PageResult<BannerViewModel>> GetAll(PagingWithKeyword request);
        Task<BannerViewModel> GetById(string id);
    }
}

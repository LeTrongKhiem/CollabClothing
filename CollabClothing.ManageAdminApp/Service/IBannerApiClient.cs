using CollabClothing.ViewModels.Catalog.Banners;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Service
{
    public interface IBannerApiClient
    {
        Task<bool> Create(BannerCreateRequest request);
        Task<bool> Delete(string id);
        Task<PageResult<BannerViewModel>> GetAll(PagingWithKeyword request);
        Task<ResultApi<BannerViewModel>> GetById(string id);
    }
}

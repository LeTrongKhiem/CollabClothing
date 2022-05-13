using CollabClothing.ViewModels.Catalog.Banners;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Banners
{
    public interface IBannerService
    {
        Task<string> Create(BannerCreateRequest request);
        Task<bool> Edit(string id, BannerEditRequest request);
        Task<bool> Delete(string id);
        Task<PageResult<BannerViewModel>> GetAllPaging(PagingWithKeyword request);
        Task<BannerViewModel> GetBannerById(string id);
    }
}

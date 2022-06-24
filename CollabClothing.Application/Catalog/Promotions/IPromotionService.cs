using CollabClothing.ViewModels.Catalog.Promotions;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Promotions
{
    public interface IPromotionService
    {
        Task<string> Create(PromotionCreateRequest request);
        Task<bool> Edit(string id, PromotionEditRequest request);
        Task<bool> Delete(string id);
        Task<PageResult<PromotionViewModel>> GetAllPaging(PagingWithKeyword request);
        Task<PromotionViewModel> GetBannerById(string id);
        Task<List<PromotionViewModel>> GetAll();
    }
}

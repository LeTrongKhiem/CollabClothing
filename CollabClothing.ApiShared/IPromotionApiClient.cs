using CollabClothing.ViewModels.Catalog.Promotions;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public interface IPromotionApiClient
    {
        Task<PageResult<PromotionViewModel>> GetAllPaging(PromotionPaging request);
        Task<bool> Create(PromotionCreateRequest request);
        Task<bool> Edit(string id, PromotionEditRequest request);
        Task<bool> Delete(string id);
        Task<List<PromotionViewModel>> GetAll();
        Task<List<PromotionViewModel>> GetPromotionByProductId(string productId);
    }
}

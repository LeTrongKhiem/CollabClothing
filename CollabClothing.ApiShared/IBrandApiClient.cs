using CollabClothing.ViewModels.Catalog.Brands;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public interface IBrandApiClient
    {
        Task<List<BrandViewModel>> GetAllBrand();
        Task<PageResult<BrandViewModel>> GetAllPaging(PagingWithKeyword request);
        Task<bool> Create(BrandCreateRequest request);
        Task<bool> Edit(string id, BrandEditRequest request);
        Task<bool> Delete(BrandDeleteRequest request);
        Task<BrandViewModel> GetById(string id);
    }
}

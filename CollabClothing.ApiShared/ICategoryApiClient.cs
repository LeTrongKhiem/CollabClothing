using CollabClothing.ViewModels.Catalog.Categories;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public interface ICategoryApiClient
    {
        Task<ResultApi<bool>> Create(CategoryCreateRequest request);
        Task<ResultApi<PageResult<CategoryViewModel>>> GetAllPaging(GetCategoryRequestPaging request);
        Task<ResultApi<bool>> Delete(string cateId);
        Task<CategoryViewModel> GetById(string cateId);
        Task<bool> Edit(string id, CategoryEditRequest request);
        Task<List<CategoryViewModel>> GetAll();
    }
}

using CollabClothing.ViewModels.Catalog.Categories;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Service
{
    public interface ICategoryApiClient
    {
        Task<ResultApi<bool>> Create(CategoryCreateRequest request);
        Task<ResultApi<PageResult<CategoryViewModel>>> GetAllPaging(GetCategoryRequestPaging request);
        Task<ResultApi<bool>> Edit(string cateId, CategoryEditRequest request);
        Task<ResultApi<bool>> Delete(string cateId);
    }
}

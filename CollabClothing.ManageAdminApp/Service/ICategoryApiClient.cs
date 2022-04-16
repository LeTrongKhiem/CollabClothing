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
        Task<ResultApi<string>> Create();
        Task<ResultApi<PageResult<CategoryViewModel>>> GetAllPaging(GetCategoryRequestPaging request);
    }
}

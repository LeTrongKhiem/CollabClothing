using CollabClothing.ViewModels.Catalog.Categories;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<string> Create(CategoryCreateRequest request);
        Task<ResultApi<int>> Edit(CategoryEditRequest request);
        Task<ResultApi<int>> Delete(string CateId);
        Task<ResultApi<PageResult<CategoryViewModel>>> GetAllPaging(GetCategoryRequestPaging request); //paing
        Task<ResultApi<CategoryViewModel>> GetCateById(string Id);
    }
}

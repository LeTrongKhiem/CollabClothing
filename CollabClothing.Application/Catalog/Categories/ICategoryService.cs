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
        Task<ResultApi<bool>> Edit(string cateId, CategoryEditRequest request);
        Task<ResultApi<bool>> Delete(string CateId);
        Task<ResultApi<PageResult<CategoryViewModel>>> GetAllPaging(GetCategoryRequestPaging request); //paing
        Task<CategoryViewModel> GetCateById(string Id);
        Task<List<CategoryViewModel>> GetAll();
        Task<List<CategoryViewModel>> GetParentCate();
        Task<List<CategoryViewModel>> GetCateChild(string parentId);
        Task<string> GetParentNameById(string id);
    }
}

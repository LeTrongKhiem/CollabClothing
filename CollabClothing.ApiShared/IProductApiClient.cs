using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public interface IProductApiClient
    {
        Task<PageResult<ProductViewModel>> GetAll(GetManageProductRequestPaging request);
        Task<bool> Create(ProductCreateRequest request);
        Task<bool> Delete(string id);
        Task<bool> Edit(string id, ProductEditRequest request);
        Task<ProductViewModel> GetById(string id);
        Task<bool> CategoryAssign(string id, CategoryAssignRequest request);
        Task<List<ProductViewModel>> GetFeaturedProducts(int take);
        Task<bool> UpdateCurrentPrice(string id, decimal newCurrentPrice);
    }
}

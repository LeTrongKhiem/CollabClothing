using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Service
{
    public interface IProductApiClient
    {
        Task<PageResult<ProductViewModel>> GetAll(GetManageProductRequestPaging request);
        Task<bool> Create(ProductCreateRequest request);
        Task<bool> Delete(string id);
    }
}

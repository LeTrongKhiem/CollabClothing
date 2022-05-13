using CollabClothing.ViewModels.Catalog.Brands;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Brands
{
    public interface IBrandService
    {
        Task<string> Create(BrandCreateRequest request);
        Task<bool> Edit(BrandEditRequest request);
        Task<bool> Delete(string brandId);
        Task<BrandViewModel> GetByBrandId(string brandId);
        Task<PageResult<BrandViewModel>> GetAllPaging(PagingWithKeyword request);

    }
}

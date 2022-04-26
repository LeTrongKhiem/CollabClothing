using CollabClothing.ViewModels.Catalog.Brands;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Brand
{
    public interface IBrandService
    {
        Task<ResultApi<bool>> Create(BrandCreateRequest request);
        Task<ResultApi<bool>> Edit(BrandEditRequest request);
        Task<ResultApi<bool>> Delete(string brandId);
        Task<ResultApi<BrandViewModel>> GetByBrandId(string brandId);
        Task<ResultApi<PageResult<BrandViewModel>>> GetAllPaging(PagingWithKeyword request);

    }
}

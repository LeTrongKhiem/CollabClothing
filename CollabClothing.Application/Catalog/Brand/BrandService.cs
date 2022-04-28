using CollabClothing.ViewModels.Catalog.Brands;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Brand
{
    public class BrandService : IBrandService
    {
        public Task<ResultApi<string>> Create(BrandCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ResultApi<bool>> Delete(string brandId)
        {
            throw new NotImplementedException();
        }

        public Task<ResultApi<bool>> Edit(BrandEditRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ResultApi<PageResult<BrandViewModel>>> GetAllPaging(PagingWithKeyword request)
        {
            throw new NotImplementedException();
        }

        public Task<ResultApi<BrandViewModel>> GetByBrandId(string brandId)
        {
            throw new NotImplementedException();
        }
    }
}

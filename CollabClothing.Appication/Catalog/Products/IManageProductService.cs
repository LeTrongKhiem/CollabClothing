using CollabClothing.Appication.Catalog.Products.Dtos;
using CollabClothing.Appication.Catalog.Products.Dtos.Manage;
using CollabClothing.Appication.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Appication.Catalog.Products
{
    public interface IManageProductService
    {
        Task<int> Create(ProductCreateRequest request);
        Task<int> Update(ProductEditRequest request);
        Task<int> Delete(string productId);
        Task<bool> UpdatePriceCurrent(string productId, decimal newPrice);
        Task<bool> UpdatePriceOld(string productId, decimal newPrice);
        Task<bool> UpdateSaleOff(string productId, int newSaleOff);
        Task AddViewCount(string productId);
        Task<List<ProductViewModel>> GetAll();
        Task<PageResult<ProductViewModel>> GetAllPaging(GetRequestPagingProduct request);
    }
}

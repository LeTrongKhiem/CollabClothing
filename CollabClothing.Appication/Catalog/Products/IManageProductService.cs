using CollabClothing.Appication.Catalog.Products.Dtos;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.ViewModels.Catalog.Products.Manage;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Http;
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
        Task<int> AddImages(string productId, List<IFormFile> files);
        Task<int> UpdateImage(string imageId, string atl);
        Task<int> RemoveImage(string imageId);
        Task AddViewCount(string productId);
        Task<List<ProductViewModel>> GetAll();
        Task<List<ProductImageViewModel>> GetListImage(string productId);
        Task<PageResult<ProductViewModel>> GetAllPaging(GetRequestPagingProduct request);
    }
}

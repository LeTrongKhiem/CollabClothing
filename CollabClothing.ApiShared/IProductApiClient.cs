using CollabClothing.ViewModels.Catalog.Color;
using CollabClothing.ViewModels.Catalog.ProductImages;
using CollabClothing.ViewModels.Catalog.Products;

using CollabClothing.ViewModels.Catalog.Sizes;

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
        Task<PageResult<ProductViewModel>> GetAll(GetManageProductRequestPaging request, string priceOrder);
        Task<bool> Create(ProductCreateRequest request);
        Task<bool> Delete(string id);
        Task<bool> Edit(string id, ProductEditRequest request);
        Task<ProductViewModel> GetById(string id);
        Task<bool> CategoryAssign(string id, CategoryAssignRequest request);
        Task<bool> SizeAssign(string id, SizeAssignRequest request);
        Task<bool> PromotionAssign(string id, PromotionAssignRequest request);
        Task<bool> ColorAssign(string id, ColorAssignRequest request);


        Task<List<ProductViewModel>> GetFeaturedProducts(int take);
        Task<List<ProductViewModel>> GetFeaturedProductsByCategory(string id, int take);
        Task<bool> UpdateCurrentPrice(string id, decimal newCurrentPrice);
        Task<bool> UpdatePriceOld(string id, decimal newPriceOld);
        Task<List<ProductViewModel>> GetAll();
        Task<List<ProductImageViewModel>> GetAllImages(string id);

        Task<string> GetBrandNameByProductId(string productId);
        Task<List<SizeViewModel>> GetSizeNameByProductId(string productId);
        Task<List<ColorViewModel>> GetColorNameByProductId(string productId);
        Task<string> GetNameProductById(string id);


        //images 

        Task<bool> CreateProductImages(string productId, ProductImageCreateRequest request);
        Task<bool> UpdateProductImages(string id, ProductImageEditRequest request);
        Task<bool> DeleteProductImages(string id);
        Task<ProductImageViewModel> GetProductImagesById(string id);
        Task<int> GetQuantityRemain(string productId);
        Task<int> GetQuantityRemain(string productId, string sizeId, string colorId);
        Task<bool> UpdateQuantityRemain(string productId, WareHouseRequest request);
        Task<WareHouseRequest> GetWareHouse(string productId);
        Task<WareHouseRequest> GetWareHouse(string productId, string sizeId);
        Task<WareHouseRequest> GetWareHouse(string productId, string sizeId, string colorId);
        Task<PageResult<ProductOrderViewModel>> GetHistoryOrder(GetManageProductRequestPaging request);
    }
}

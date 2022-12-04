using CollabClothing.ViewModels.Catalog.Color;
using CollabClothing.ViewModels.Catalog.ProductImages;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.ViewModels.Catalog.Sizes;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Products
{
    public interface IManageProductService
    {
        //method create product
        Task<string> Create(ProductCreateRequest request);
        Task<int> Update(string id, ProductEditRequest request);
        Task<int> Delete(string productId);
        Task<ProductViewModel> GetProductById(string productId);
        Task<bool> UpdatePriceCurrent(string productId, decimal newPrice);
        Task<bool> UpdatePriceOld(string productId, decimal newPrice);
        Task<bool> UpdateSaleOff(string productId, int newSaleOff);
        Task<List<ProductViewModel>> GetFeaturedProducts(int take);

        Task<List<ProductViewModel>> GetRelatedProduct(string productId, int take);
        Task<List<ProductViewModel>> GetFeaturedProductsCategory(string idCate, int take);
        Task<PageResult<ProductViewModel>> GetProductByCategory(GetPublicProductRequestPaging request);
        string GetBrandByProductId(string productId);
        //loadmore
        Task<PageResult<ProductViewModel>> GetProductLoadMore(int amount, string cateId);

        //Task<List<ProductViewModel>> GetFeaturedProductsCategory(string idCate, int take);
        //Task<PageResult<ProductViewModel>> GetProductByCategory(GetPublicProductRequestPaging request);


        // method productimage

        Task<string> AddImages(string productId, ProductImageCreateRequest request);
        Task<int> UpdateImage(string imageId, ProductImageEditRequest request);
        Task<int> RemoveImage(string imageId);
        Task<List<ProductImageViewModel>> GetListImage(string productId);
        Task<ProductImageViewModel> GetProductImageById(string imageId);
        //assign category
        Task<bool> CategoryAssign(string id, CategoryAssignRequest request);
        //assign size
        Task<bool> SizeAssign(string id, SizeAssignRequest request);
        Task<bool> PromotionAssign(string id, PromotionAssignRequest request);
        Task<bool> ColorAssign(string id, ColorAssignRequest request);

        Task<int> GetQuantityRemain(string productId);
        Task<int> GetQuantityRemain(string productId, string sizeId, string colorId);
        Task<bool> UpdateQuantityRemainProduct(string productId, WareHouseRequest request);
        Task<WareHouseRequest> GetWareHouse(string productId);
        Task<WareHouseRequest> GetWareHouse(string productId, string sizeId);
        Task<WareHouseRequest> GetWareHouse(string productId, string sizeId, string colorId);

        // Task AddViewCount(string productId);
        Task<List<ProductViewModel>> GetAll();
        Task<PageResult<ProductViewModel>> GetAllPaging(GetManageProductRequestPaging request);
        Task<PageResult<ProductViewModel>> GetAllPaging(GetManageProductRequestPaging request, string priceOrder);

        //method phan chia product voi category

        #region Get Name
        List<SizeViewModel> GetNameSize(string productId);
        List<ColorViewModel> GetColorSize(string productId);
        Task<string> GetNameProductById(string id);
        #endregion

        #region TMDT
        Task<PageResult<ProductOrderViewModel>> GetOrderHistory(Guid userId, GetManageProductRequestPaging request);
        #endregion
    }
}

using CollabClothing.ApiShared;
using CollabClothing.ViewModels.Catalog.ProductImages;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.ViewModels.Catalog.Sizes;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Controllers
{
    public class ProductController : BaseController
    {
        #region Constructor
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IBrandApiClient _brandApiClient;
        private readonly ISizeApiClient _sizeApiClient;
        private readonly IPromotionApiClient _promotionApiClient;
        public ProductController(IProductApiClient productApiClient, IConfiguration configuration, ICategoryApiClient categoryApiClient, IBrandApiClient brandApiClient, ISizeApiClient sizeApiClient, IPromotionApiClient promotionApiClient)
        {
            _productApiClient = productApiClient;
            _configuration = configuration;
            _categoryApiClient = categoryApiClient;
            _brandApiClient = brandApiClient;
            _sizeApiClient = sizeApiClient;
            _promotionApiClient = promotionApiClient;
        }
        #endregion
        #region Index
        public async Task<IActionResult> Index(string keyword, string categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetManageProductRequestPaging()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = categoryId
            };
            var data = await _productApiClient.GetAll(request);
            ViewBag.Keyword = keyword;
            var categories = await _categoryApiClient.GetAll();
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.CategoryId,
                Selected = categoryId == x.CategoryId && categoryId != null
            });
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }
        #endregion
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryApiClient.GetAll();
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.CategoryId,
                Selected = x.CategoryId != null
            });
            var brand = await _brandApiClient.GetAllBrand();
            ViewBag.Brands = brand.Select(x => new SelectListItem()
            {
                Text = x.NameBrand,
                Value = x.Id,
                Selected = x.Id != null
            });
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _productApiClient.Create(request);
            if (result)
            {
                TempData["result"] = "Thêm sản phẩm mới thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Thêm sản phẩm mới thất bại");
            return View(request);
        }
        #endregion
        #region Delete
        [HttpGet]
        public IActionResult Delete(string Id)
        {
            var product = new ProductDeleteRequest()
            {
                Id = Id
            };
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(ProductDeleteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _productApiClient.Delete(request.Id);
            if (result)
            {
                TempData["result"] = "Xóa thành công sản phẩm";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Xóa thất bại sản phẩm");
            return View(request);
        }
        #endregion
        #region Update
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var product = _productApiClient.GetById(id);
            if (product != null)
            {
                var productResult = product.Result;
                var editProduct = new ProductEditRequest()
                {
                    ProductName = productResult.ProductName,
                    Details = productResult.Details,
                    Description = productResult.Description,
                    BrandId = productResult.BrandId,
                    Slug = productResult.Slug,
                    ImagePath = productResult.ThumbnailImage,
                    Consumer = productResult.Consumer,
                    Form = productResult.Form,
                    Type = productResult.Type,
                    Cotton = productResult.Cotton,
                    MadeIn = productResult.MadeIn
                };
                return View(editProduct);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit(string id, [FromForm] ProductEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            ViewBag.Product = request;
            var result = await _productApiClient.Edit(id, request);
            if (result)
            {
                TempData["result"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Cập nhật thất bại");
            return View(request);
        }
        #endregion
        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var product = await _productApiClient.GetById(id);
            if (product == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(product);
        }
        #endregion
        #region CategoryAssign
        [HttpGet]
        public async Task<IActionResult> CategoriesAssign(string id)
        {
            var categoriesAssignRequest = await GetCategoriesAssignRequest(id);
            return View(categoriesAssignRequest);
        }

        private async Task<CategoryAssignRequest> GetCategoriesAssignRequest(string id)
        {
            var product = await _productApiClient.GetById(id);
            var categories = await _categoryApiClient.GetAll();
            var categoryAssignRequest = new CategoryAssignRequest();
            foreach (var category in categories)
            {
                categoryAssignRequest.Categories.Add(new SelectItem()
                {
                    Id = category.CategoryId,
                    Name = category.CategoryName,
                    Selected = product.Categories.Contains(category.CategoryName)
                });
            }
            return categoryAssignRequest;
        }
        [HttpPost]
        public async Task<IActionResult> CategoriesAssign(CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _productApiClient.CategoryAssign(request.Id, request);
            if (result)
            {
                TempData["result"] = "Cập nhật danh mục sản phẩm thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Error", "");
            var rolesAssignRequest = GetCategoriesAssignRequest(request.Id);
            return View(rolesAssignRequest);
        }
        #endregion
        #region PromotionAssign
        [HttpGet]
        public async Task<IActionResult> PromotionsAssign(string id)
        {
            var promotionsAssignRequest = await GetPromotionsAssignRequest(id);
            return View(promotionsAssignRequest);
        }

        private async Task<PromotionAssignRequest> GetPromotionsAssignRequest(string id)
        {
            var product = await _productApiClient.GetById(id);
            var promotions = await _promotionApiClient.GetAll();
            var promotionAssignRequest = new PromotionAssignRequest();
            foreach (var promotion in promotions)
            {
                promotionAssignRequest.Promotions.Add(new SelectItem()
                {
                    Id = promotion.Id,
                    Name = promotion.NamePromotion,
                    Selected = product.Promotions.Contains(promotion.NamePromotion)
                });
            }
            return promotionAssignRequest;
        }
        [HttpPost]
        public async Task<IActionResult> PromotionsAssign(PromotionAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _productApiClient.PromotionAssign(request.Id, request);
            if (result)
            {
                TempData["result"] = "Cập nhật danh mục sản phẩm thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Error", "");
            var promotionsAssignRequest = GetCategoriesAssignRequest(request.Id);
            return View(promotionsAssignRequest);
        }
        #endregion
        #region SizeAssign
        [HttpGet]
        public async Task<IActionResult> SizesAssign(string id)
        {
            var sizeAssign = await GetSizesAssignRequest(id);
            return View(sizeAssign);
        }

        private async Task<SizeAssignRequest> GetSizesAssignRequest(string id)
        {
            var product = await _productApiClient.GetById(id);
            var sizes = await _sizeApiClient.GetAll();
            var sizeAssign = new SizeAssignRequest();
            foreach (var size in sizes)
            {
                sizeAssign.Sizes.Add(new SelectItem()
                {
                    Id = size.Id,
                    Name = size.Name,
                    Selected = product.Sizes.Contains(size.Name)
                });
            }
            return sizeAssign;
        }
        [HttpPost]
        public async Task<IActionResult> SizesAssign(SizeAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _productApiClient.SizeAssign(request.Id, request);
            if (result)
            {
                TempData["result"] = "Cập nhật danh mục sản phẩm thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Error", "");
            var sizesAssignRequest = GetCategoriesAssignRequest(request.Id);
            return View(sizesAssignRequest);
        }
        #endregion
        #region UpdateCurrentPrice
        [HttpGet]
        public async Task<IActionResult> UpdateCurrentPrice(string id)
        {
            var product = await _productApiClient.GetById(id);
            if (product != null)
            {
                var editProduct = new ProductEditRequest()
                {
                    PriceCurrent = product.PriceCurrent
                };
                return View(editProduct);
            }
            return RedirectToAction("Error", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateCurrentPrice(string id, ProductEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _productApiClient.UpdateCurrentPrice(id, request.PriceCurrent);
            if (result)
            {
                TempData["result"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }
            return View(result);
        }
        #endregion

        #region UpdateOldPrice
        [HttpGet]
        public async Task<IActionResult> UpdateOldPrice(string id)
        {
            var product = await _productApiClient.GetById(id);
            if (product != null)
            {
                var editProduct = new ProductEditRequest()
                {
                    PriceOld = product.PriceOld
                };
                return View(editProduct);
            }
            return RedirectToAction("Error", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateOldPrice(string id, ProductEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _productApiClient.UpdatePriceOld(id, request.PriceOld);
            if (result)
            {
                TempData["result"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }
            return View(result);
        }
        #endregion
        #region GetListImages
        [HttpGet]
        public async Task<IActionResult> GetListImages(string id)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _productApiClient.GetAllImages(id);


            if (TempData["resultImages"] != null)
            {
                ViewBag.ResultImage = TempData["resultImages"];
            }
            else
            {
                TempData["idPrevious"] = id;
                ViewData["IdPrevious"] = id;
                ViewBag.IdPrevious = id;
            }
            return View(result);
        }
        #endregion
        #region AddImages
        [HttpGet]
        [Route("Product/CreateImages/{productId}")]
        public IActionResult CreateImages(string productId)
        {
            ViewBag.IdPrevious = productId;
            var productImagesCreate = new ProductImageCreateRequest()
            {
                ProductId = productId
            };
            return View(productImagesCreate);
        }
        [HttpPost]
        [Route("Product/CreateImages/{productId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateImages([FromRoute] string productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            ViewBag.IdPrevious = TempData["idPrevious"];
            //var result = await _productApiClient.CreateProductImages(productId, request);

            ////ViewBag.IdPrevious = TempData["idPrevious"];
            //string productId = ViewBag.IdPrevious;
            //var productId = TempData["idPrevious"].ToString();
            var result = await _productApiClient.CreateProductImages(productId, request);
            if (result)
            {
                TempData["resultImages"] = "Tạo hình ảnh thành công";
                ViewBag.IdPrevious = TempData["idPrevious"];
                return RedirectToAction("GetListImages", new { id = productId });
            }
            ModelState.AddModelError("", "Thêm hình ảnh thất bại");
            return View(request);
        }
        #endregion
        #region UpdateProductImages
        [HttpGet]
        public async Task<IActionResult> UpdateProductImages(string id)
        {
            var productImages = await _productApiClient.GetProductImagesById(id);
            if (productImages != null)
            {
                var productResult = productImages;
                var editProductImages = new ProductImageEditRequest()
                {
                    Id = productResult.Id,
                    Alt = productResult.Alt,
                    File = productResult.ImageFile,
                    IsThumbnail = productResult.IsThumbnail,
                    Path = productResult.Path
                };
                //ViewBag.IdPrevious = TempData["idPrevious"];
                ViewData["IdPrevious"] = TempData["idPrevious"];
                ViewBag.IdPrevious = TempData["idPrevious"];
                return View(editProductImages);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProductImages(string id, [FromForm] ProductImageEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _productApiClient.UpdateProductImages(id, request);
            if (result)
            {
                TempData["resultImages"] = "Sửa hình ảnh thành công";
                ViewData["IdPrevious"] = TempData["idPrevious"];
                ViewBag.IdPrevious = TempData["idPrevious"];
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Sửa hình ảnh thất bại");
            return View(request);
        }
        #endregion
        #region DeleteProductImages
        [HttpGet]
        public IActionResult DeleteProductImages(string id)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var product = new ProductImagesDeleteRequest()
            {
                Id = id
            };
            ViewData["IdPrevious"] = TempData["idPrevious"];
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProductImages(ProductImagesDeleteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _productApiClient.DeleteProductImages(request.Id);
            if (result)
            {
                TempData["resultImages"] = "Xóa hình ảnh thành công";
                ViewData["IdPrevious"] = TempData["idPrevious"];
                return RedirectToAction("Index");

            }
            ModelState.AddModelError("", "Xóa hình ảnh thất bại");
            return View(request);
        }
        #endregion

        #region Update Quantity Remain
        [HttpGet]
        public async Task<IActionResult> UpdateQuantityRemain(string id, string sizeId)
        {
            var sizes = await _sizeApiClient.GetAll();
            ViewBag.Size = sizes.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id,
                Selected = x.Id != null
            });
            var remain = await _productApiClient.GetWareHouse(id, sizeId);
            if (remain == null)
            {
                var wareHouse = new WareHouseRequest()
                {
                    ProductId = id,
                    SizeId = sizeId
                };
                return View(wareHouse);
            }
            else
            {
                var wareHouse = new WareHouseRequest()
                {
                    Quantity = remain.Quantity,
                    Id = remain.Id,
                    ColorId = remain.ColorId,
                    ProductId = remain.ProductId,
                    SizeId = remain.SizeId
                };
                if (TempData["result"] != null)
                {
                    ViewBag.SuccessMsg = TempData["result"];
                }
                return View(wareHouse);
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdateQuantityRemain(WareHouseRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _productApiClient.UpdateQuantityRemain(request.ProductId, request);
            if (result)
            {
                TempData["result"] = "Cập nhật số lượng sản phẩm thành công";
                return RedirectToAction("UpdateQuantityRemain");
            }
            return View(result);
        }
        #endregion

    }
}

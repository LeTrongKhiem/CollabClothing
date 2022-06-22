using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.ViewModels.Catalog.ProductImages;
using CollabClothing.Application.Catalog.Products;
using System;
using Microsoft.AspNetCore.Authorization;

namespace CollabClothing.BackendApi.Controllers
{
    //api/products
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;
        public ProductsController(IPublicProductService publicProductService, IManageProductService manageProductService)
        {
            _publicProductService = publicProductService;
            _manageProductService = manageProductService;
        }
        //api get all product 
        //url mac dinh cua get http://localhost:port/controller
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var product = await _publicProductService.GetAll();
            return Ok(product);
        }
        //api paging product truyen vao categoryId pageindex va pagesize
        //http://localhost:port/products/public-paging
        [HttpGet("public-paging")]
        public async Task<IActionResult> Get([FromQuery] GetPublicProductRequestPaging request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = await _publicProductService.GetAllByCategoryId(request);
            if (product == null)
            {
                return BadRequest("Not found");
            }
            return Ok(product);
        }
        //get paging product admin
        [HttpGet("paging")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductRequestPaging request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = await _manageProductService.GetAllPaging(request);
            if (product == null)
            {
                return BadRequest("Not found");
            }
            return Ok(product);
        }
        [HttpGet("getnameproduct/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNameProductById(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = await _manageProductService.GetNameProductById(id);
            if (product == null)
            {
                return BadRequest("Not found");
            }
            return Ok(product);
        }
        [HttpGet("getbrand/{productId}")]
        [AllowAnonymous]
        public IActionResult GetBrandNameByProductId(string productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = _manageProductService.GetBrandByProductId(productId);
            if (product == null)
            {
                return BadRequest("Not found");
            }
            return Ok(product);
        }
        //http://localhost:5001/products/id
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetById(string productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = await _manageProductService.GetProductById(productId);
            if (product == null)
            {
                return BadRequest("Cannot find product");
            }
            return Ok(product);
        }
        [HttpGet("sizename/{productId}")]
        public IActionResult GetSizeName(string productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = _manageProductService.GetNameSize(productId);
            if (product == null)
            {
                return BadRequest("Cannot find product");
            }
            return Ok(product);
        }
        [HttpGet("featured/{take}")]
        public async Task<IActionResult> GetFeaturedProducts(int take)
        {
            var product = await _manageProductService.GetFeaturedProducts(take);
            if (product == null)
            {
                return BadRequest("Cannot find products");
            }
            return Ok(product);
        }

        [HttpGet("featured/{id}/{take}")]
        public async Task<IActionResult> GetFeaturedProductsCategory(string id, int take)
        {
            var product = await _manageProductService.GetFeaturedProductsCategory(id, take);
            if (product == null)
            {
                return BadRequest("Cannot find products");
            }
            return Ok(product);
        }
        [HttpGet("related/{productId}/{take}")]
        public async Task<IActionResult> GetRelatedProductsById(string productId, int take)
        {
            var products = await _manageProductService.GetRelatedProduct(productId, take);
            if (products == null)
            {
                return BadRequest("Not found");
            }
            return Ok(products);
        }
        [HttpGet("/category/{cateId}")]
        public async Task<IActionResult> GetProductByCategoryId(string cateId)
        {
            var product = await _publicProductService.GetProductByCategoryId(cateId);
            if (product == null)
            {
                return BadRequest("Cannot find product");
            }
            return Ok(product);
        }
        //
        [HttpPost]
        [Consumes("multipart/form-data")] // accept 1 form data fromform
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _manageProductService.Create(request);
            if (productId.Equals("") || productId == null)
            {
                return BadRequest();
            }
            var product = await _manageProductService.GetProductById(productId);
            return CreatedAtAction(nameof(productId), new { id = productId }, product);
        }
        [HttpPut("{id}/categories")]
        public async Task<IActionResult> CategoriesAssign(string id, CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _manageProductService.CategoryAssign(id, request);
            if (!result)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut("sizes/{id}")]
        public async Task<IActionResult> SizesAssign(string id, SizeAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _manageProductService.SizeAssign(id, request);
            if (!result)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //update product
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(string id, [FromForm] ProductEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var affectedResult = await _manageProductService.Update(id, request);
            if (affectedResult == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
        //delete product
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(string productId)
        {
            var affectedResult = await _manageProductService.Delete(productId);
            if (affectedResult == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPut("newPriceCurrent/{productId}/{newCurrentPrice}")]
        public async Task<IActionResult> UpdateCurrentPrice(string productId, decimal newCurrentPrice)
        {
            var isSuccess = await _manageProductService.UpdatePriceCurrent(productId, newCurrentPrice);
            if (isSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("newPriceOld/{productId}/{newOldPrice}")]
        public async Task<IActionResult> UpdateOldPrice(string productId, decimal newOldPrice)
        {
            var isSuccess = await _manageProductService.UpdatePriceOld(productId, newOldPrice);
            if (isSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }
        //method update sale off product
        [HttpPut("newSaleOff/{productId}/{newSaleOff}")]
        public async Task<IActionResult> UpdateSaleOff(string productId, int newSaleOff)
        {
            var isSuccess = await _manageProductService.UpdateSaleOff(productId, newSaleOff);
            if (isSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }


        //api image
        [HttpPost("{productId}/images")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddProductImage(string productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var productImageId = await _manageProductService.AddImages(productId, request);
            if (productImageId == null || productImageId.Equals(""))
            {
                return BadRequest();
            }
            var image = await _manageProductService.GetProductImageById(productImageId);
            return CreatedAtAction(nameof(image), new { id = productImageId }, image);
        }
        [HttpDelete("images/{imageId}")]
        public async Task<IActionResult> DeleteProductImage(string imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var affectedResult = await _manageProductService.RemoveImage(imageId);
            if (affectedResult == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
        //method update product image
        [HttpPut("images/{imageId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProductImage(string imageId, [FromForm] ProductImageEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var affectedResult = await _manageProductService.UpdateImage(imageId, request);
            if (affectedResult == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpGet("images/{productId}")]
        public async Task<IActionResult> GetListImagesByProductId(string productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _manageProductService.GetListImage(productId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpGet("images/product/{productImagesId}")]
        public async Task<IActionResult> GetListImagesByProductImagesId(string productImagesId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _manageProductService.GetProductImageById(productImagesId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
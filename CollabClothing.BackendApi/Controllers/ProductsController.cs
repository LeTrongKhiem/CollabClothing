using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.ViewModels.Catalog.ProductImages;
using CollabClothing.Application.Catalog.Products;

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
            var product = await _publicProductService.GetAllByCategoryId(request);
            return Ok(product);
        }
        //http://localhost:5001/products/id
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetById(string productId)
        {
            var product = await _manageProductService.GetProductById(productId);
            if (product == null)
            {
                return BadRequest("Cannot find product");
            }
            return Ok(product);
        }
        //
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var productId = await _manageProductService.Create(request);
            if (productId.Equals("") || productId == null)
            {
                return BadRequest();
            }
            var product = await _manageProductService.GetProductById(productId);
            return CreatedAtAction(nameof(productId), new { id = productId }, product);
        }
        //update product
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var affectedResult = await _manageProductService.Update(request);
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
    }
}
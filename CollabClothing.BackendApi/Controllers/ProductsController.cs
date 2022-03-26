using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CollabClothing.Appication.Catalog.Products;
using CollabClothing.ViewModels.Catalog.Products;

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
        //url mac dinh cua get http://localhost:port/controller
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var product = await _publicProductService.GetAll();
            return Ok(product);
        }
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
        public async Task<IActionResult> Create([FromBody] ProductCreateRequest request)
        {
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
        public async Task<IActionResult> Update([FromBody] ProductEditRequest request)
        {
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
        [HttpPut("price/{productId}/{newPriceCurrent}")]
        public async Task<IActionResult> UpdateCurrentPrice(string productId, decimal newCurrentPrice)
        {
            var isSuccess = await _manageProductService.UpdatePriceCurrent(productId, newCurrentPrice);
            if (isSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("price/{productId}/{newPriceOld}")]
        public async Task<IActionResult> UpdateOldPrice(string productId, decimal newOldPrice)
        {
            var isSuccess = await _manageProductService.UpdatePriceCurrent(productId, newOldPrice);
            if (isSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNerCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CollabClothing.BackendApi.Controllers
{
    //api/products
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("Demo ok");
        }
    }
}
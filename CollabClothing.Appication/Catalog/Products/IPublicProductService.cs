
using CollabClothing.Appication.Catalog.Products.Dtos.Public;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Appication.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetRequestPagingProduct request);
    }
}

using CollabClothing.Appication.Catalog.Products.Dtos;
using CollabClothing.Appication.Catalog.Products.Dtos.Public;
using CollabClothing.Appication.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Appication.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        public PageResult<ProductViewModel> GetAllByCategoryId(GetRequestPagingProduct request)
        {
            throw new NotImplementedException();
        }
    }
}

using CollabClothing.Appication.Catalog.Products.Dtos;
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
        public PageViewModel<ProductViewModel> GetAllByCategoryId(string categoryId, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}

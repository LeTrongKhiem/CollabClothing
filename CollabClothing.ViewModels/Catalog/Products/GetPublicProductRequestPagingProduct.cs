using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Products
{
    public class GetPublicProductRequestPagingProduct : PagingRequestBase
    {
        public string? CategoryId { get; set; }
    }

}

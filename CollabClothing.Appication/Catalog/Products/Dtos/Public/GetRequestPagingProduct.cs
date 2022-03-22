using CollabClothing.Appication.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Appication.Catalog.Products.Dtos.Public
{
    public class GetRequestPagingProduct : PagingRequestBase
    {
        public string CategoryId { get; set; }
    }
}

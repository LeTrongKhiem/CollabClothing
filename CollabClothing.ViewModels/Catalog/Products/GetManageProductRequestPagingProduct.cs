using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Products
{
    public class GetManageProductRequestPagingProduct : PagingRequestBase
    {
        public string Keyword { get; set; }
        public List<string> CategoryIds { get; set; }
    }
}
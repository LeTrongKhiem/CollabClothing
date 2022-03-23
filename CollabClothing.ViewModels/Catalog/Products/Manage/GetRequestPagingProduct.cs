using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Products.Manage
{
    public class GetRequestPagingProduct : PagingRequestBase
    {
        public string Keyword { get; set; }
        public List<string> CategoryIds { get; set; }
    }
}

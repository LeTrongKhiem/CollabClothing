using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Products
{
    public class GetManageProductRequestPaging : PagingRequestBase
    {
        public string Keyword { get; set; }
        public string? CategoryId { get; set; }
        public string? Slug { get; set; }
        public string? BrandId { get; set; }
    }
}
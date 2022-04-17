using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Categories
{
    public class GetCategoryRequestPaging : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}

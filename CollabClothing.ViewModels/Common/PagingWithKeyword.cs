using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Common
{
    public class PagingWithKeyword : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}

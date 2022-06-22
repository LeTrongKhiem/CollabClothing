using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Common
{
    public class PagingCart : PagingRequestBase
    {
        public string Keyword { get; set; }
        public bool Status { get; set; }
    }
}

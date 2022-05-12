using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Banners
{
    public interface IBannerService
    {
        Task<string> Create();
    }
}

using CollabClothing.ViewModels.Catalog.Sizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Sizes
{
    public interface ISizeService
    {
        Task<List<SizeViewModel>> GetAllSize();
        Task<string> GetNameSize(string id);

    }
}

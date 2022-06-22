using CollabClothing.ViewModels.Catalog.Sizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public interface ISizeApiClient
    {
        public Task<List<SizeViewModel>> GetAll();
        public Task<string> GetNameSize(string id);
    }
}

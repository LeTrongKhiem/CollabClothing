using CollabClothing.ViewModels.Catalog.Color;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ApiShared
{
    public interface IColorApiClient
    {
        public Task<List<ColorViewModel>> GetAll();
        public Task<string> GetNameColor(string id);
    }
}

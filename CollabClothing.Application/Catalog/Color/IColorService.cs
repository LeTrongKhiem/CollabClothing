using CollabClothing.ViewModels.Catalog.Color;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Color
{
    public interface IColorService
    {
        Task<List<ColorViewModel>> GetAllColor();
        Task<string> GetNameColor(string id);
    }
}

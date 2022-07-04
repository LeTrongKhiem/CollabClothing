using CollabClothing.Data.EF;
using CollabClothing.ViewModels.Catalog.Color;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Color
{
    public class ColorService : IColorService
    {
        private readonly CollabClothingDBContext _context;
        public ColorService(CollabClothingDBContext context)
        {
            _context = context;
        }
        public async Task<List<ColorViewModel>> GetAllColor()
        {
            var query = from s in _context.Colors
                        select new ColorViewModel()
                        {
                            Id = s.Id,
                            Name = s.NameColor
                        };
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<string> GetNameColor(string id)
        {
            var query = (from s in _context.Colors where s.Id == id select s.NameColor).FirstOrDefault();
            return query;
        }
    }
}

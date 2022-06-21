using CollabClothing.Data.EF;
using CollabClothing.ViewModels.Catalog.Sizes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Sizes
{
    public class SizeService : ISizeService
    {
        private readonly CollabClothingDBContext _context;
        public SizeService(CollabClothingDBContext context)
        {
            _context = context;
        }
        public async Task<List<SizeViewModel>> GetAllSize()
        {
            var query = from s in _context.Sizes
                        select new SizeViewModel()
                        {
                            Id = s.Id,
                            Name = s.NameSize
                        };
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<string> GetNameSize(string id)
        {
            var query = (from s in _context.Sizes where s.Id == id select s.NameSize).FirstOrDefault();
            return query;
        }
    }
}

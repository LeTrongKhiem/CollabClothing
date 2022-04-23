using CollabClothing.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Data.Dtos
{
    public class CategoryDTO
    {
        public string Id { get; set; }
        public string NameCategory { get; set; }
        public string ParentId { get; set; }
        public string Icon { get; set; }
        public int Level { get; set; }
        public bool IsShowWeb { get; set; }
        public string Slug { get; set; }
    }
}

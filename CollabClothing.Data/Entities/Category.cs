using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Data.Entities
{
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsShowWeb { get; set; }
        public int Level { get; set; }
        public string? ParentId { get; set; }
        public string Slug { get; set; }
        public string Icon { get; set; }

    }
}

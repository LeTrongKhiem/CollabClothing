using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Catalog.Categories
{
    public class CategoryEditRequest
    {
        [Display(Name = "Tên danh mục")]
        public string CategoryName { get; set; }
        [Display(Name = "Danh mục cha")]
        public string ParentId { get; set; }
        [Display(Name = "Level")]
        public int Level { get; set; }
        [Display(Name = "Hiển thị trên Web")]
        public bool IsShowWeb { get; set; }
        [Display(Name = "Slug")]
        public string Slug { get; set; }
        [Display(Name = "Hình ảnh")]
        public IFormFile Icon { get; set; }
    }
}

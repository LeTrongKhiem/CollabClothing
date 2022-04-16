using System;
using System.ComponentModel.DataAnnotations;

namespace CollabClothing.ViewModels.Catalog.Categories
{
    public class CategoryViewModel
    {
        public string CategoryId { get; set; }
        [Display(Name = "Tên danh mục")]
        public string CategoryName { get; set; }
        [Display(Name = "Mã danh mục cha")]
        public string ParentId { get; set; }
        [Display(Name = "Level")]
        public int Level { get; set; }
        [Display(Name = "ShowWeb")]
        public bool IsShowWeb { get; set; }
        public string Slug { get; set; }
        public string Icon { get; set; }
    }
}
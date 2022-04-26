using CollabClothing.Data.Dtos;
using CollabClothing.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Data.Extensions
{
    public static class MyExtensionMapping
    {
        #region CateMappingDTO
        public static void CateMapping(this Category category, CategoryDTO categoryDTO)
        {
            category.Id = categoryDTO.Id;
            category.NameCategory = categoryDTO.NameCategory;
            category.ParentId = categoryDTO.ParentId;
            category.Icon = categoryDTO.Icon;
            category.Level = categoryDTO.Level;
            category.IsShowWeb = categoryDTO.IsShowWeb;
            category.Slug = categoryDTO.Slug;
        }
        #endregion

        #region BrandMappingDTO
        public static void BrandMapping(this Brand brand, BrandDTO brandDTO)
        {
            brand.Id = brandDTO.Id;
            brand.NameBrand = brandDTO.NameBrand;
            brand.Info = brandDTO.Info;
            brand.Images = brandDTO.Images;
            brand.Slug = brandDTO.Slug;
        }
        #endregion  

    }
}

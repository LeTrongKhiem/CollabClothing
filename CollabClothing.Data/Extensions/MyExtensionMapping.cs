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
            brand.Images = brandDTO.Images;
            brand.Info = brandDTO.Info;
            brand.NameBrand = brandDTO.NameBrand;
            brand.Slug = brandDTO.Slug;
        }
        #endregion
        #region BannerMappingDTO
        public static void BannerMapping(this Banner banner, BannerDTO bannerDTO)
        {
            banner.Id = bannerDTO.Id;
            banner.NameBanner = bannerDTO.NameBanner;
            banner.Images = bannerDTO.Images;
            banner.Alt = bannerDTO.Alt;
            banner.Text = bannerDTO.Text;
            banner.TypeBannerId = bannerDTO.TypeBannerId;
        }
        #endregion
    }
}

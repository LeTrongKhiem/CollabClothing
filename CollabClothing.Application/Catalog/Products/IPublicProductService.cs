﻿
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductRequestPaging request);

        Task<List<ProductViewModel>> GetAll();
        Task<List<ProductViewModel>> GetProductByCategoryId(string cateId);
    }
}

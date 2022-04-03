﻿using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;
using System.Net.Http.Headers;
using CollabClothing.Utilities.Exceptions;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.ViewModels.Common;
using CollabClothing.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollabClothing.Application.Common;
using CollabClothing.ViewModels.Catalog.ProductImages;
using Microsoft.AspNetCore.Hosting;
using CollabClothing.Data.EF;

namespace CollabClothing.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly CollabClothingDBContext _context;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";
        public ManageProductService(CollabClothingDBContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }


        // public async Task AddViewCount(string productId)
        // {
        //     var product = await _context.Products.FindAsync(productId);
        //     product.ViewCount += 1;
        //     await _context.SaveChangesAsync();
        // }
        //create product ProductCreateRequest la ham duoc tao ben CollabClothing.ViewModels dung de the hien cac thuoc tinh maf nguoi dung co the nhap 
        //de tao nen 1 san pham
        public async Task<string> Create(ProductCreateRequest request)
        {
            Guid g = Guid.NewGuid();
            var product = new Product()
            {
                Id = g.ToString(),
                ProductName = request.ProductName,
                PriceCurrent = request.PriceCurrent,
                PriceOld = request.PriceOld,
                SaleOff = request.SaleOff,
                BrandId = request.BrandId,
                SoldOut = request.SoldOut,
                Installment = request.Installment,
                Description = request.Description,
                Slug = request.Slug,
            };
            var ProductMapCategory = new ProductMapCategory()
            {
                ProductId = product.Id,
                CategoryId = request.CategoryId
            };
            var Thumbnail = new ProductImage()
            {
                Id = g.ToString(),
                ProductId = product.Id,
                Alt = product.ProductName

            };
            if (request.ThumbnailImage != null)
            {
                Thumbnail.Path = await this.SaveFile(request.ThumbnailImage);
            }
            else
            {
                Thumbnail.Path = "no-image";
            }
            _context.Products.Add(product);
            _context.ProductMapCategories.Add(ProductMapCategory);
            _context.ProductImages.Add(Thumbnail);
            await _context.SaveChangesAsync();
            return product.Id;
        }
        //method dung de delete product khai bao bien product dung de tim kiem product bang id
        //va bien image tim cac hinh anh cos ma san pham tuong ung duyet qua va xoa
        public async Task<int> Delete(string productId)
        {
            var productMapCate = await _context.ProductMapCategories.FirstOrDefaultAsync(x => x.ProductId == productId);
            var product = await _context.Products.FindAsync(productId);
            if (productId == null)
                throw new CollabException($"Cannot find a product: {productId}");
            var images = _context.ProductImages.Where(i => i.ProductId == productId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.Path);
            }

            _context.ProductMapCategories.Remove(productMapCate);
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(ProductEditRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var imagePath = (from p in _context.Products
                             join pimg in _context.ProductImages on p.Id equals pimg.ProductId
                             where p.Id == product.Id
                             select pimg.Path).ToString();
            var image = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductId == product.Id);
            // var productDetail = await _context.ProductDetails.FirstOrDefaultAsync(x => x.ProductId == product.Id);
            if (product == null)
            {
                throw new CollabException($"Cannot find product with Id: {request.Id}");
            }
            product.Id = request.Id;
            product.ProductName = request.ProductName;
            // productDetail.Details = request.Details;
            product.Description = request.Description;
            product.BrandId = request.BrandId;
            //delete old image file
            string fullPath = "wwwroot" + image.Path;
            if (File.Exists(fullPath))
            {
                await Task.Run(() =>
                {
                    File.Delete(fullPath);
                });
            }
            product.Slug = request.Slug;
            //save image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.ProductId == request.Id);

                if (thumbnailImage != null)
                {
                    // thumbnailImage.Id = request.productImage.Id;
                    thumbnailImage.Path = await this.SaveFile(request.ThumbnailImage);
                    thumbnailImage.Alt = request.ProductName;
                    _context.ProductImages.Update(thumbnailImage);
                }
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<ProductViewModel> GetProductById(string productId)
        {
            var product = await _context.Products.FindAsync(productId);
            var categories = await (from c in _context.Categories
                                    join pmc in _context.ProductMapCategories on c.Id equals pmc.CategoryId
                                    where pmc.ProductId == productId
                                    select c.NameCategory).ToListAsync();
            var image = await _context.ProductImages.Where(x => x.ProductId == productId).FirstOrDefaultAsync();
            if (product == null)
            {
                throw new CollabException($"Cannot find product with id: {productId}");
            }
            var viewModel = new ProductViewModel()
            {
                Id = product.Id,
                ProductName = product.ProductName,
                PriceCurrent = product.PriceCurrent,
                PriceOld = product.PriceOld,
                SaleOff = product.SaleOff,
                Description = product.Description,
                BrandId = product.BrandId,
                Installment = product.Installment,
                Slug = product.Slug,
                SoldOut = product.SoldOut,
                Categories = categories,
                ThumbnailImage = image != null ? image.Path : "no-image.jpg",
            };
            return viewModel;
        }

        public async Task<List<ProductViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }
        //method phan trang
        //GetRequestPagingProduct truyen vao keyword la chuoi tim kiem va 1 list cac categoryid
        public async Task<PageResult<ProductViewModel>> GetAllPaging(GetManageProductRequestPaging request)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pmc in _context.ProductMapCategories on p.Id equals pmc.ProductId into ppmc
                        from pmc in ppmc.DefaultIfEmpty()
                        join c in _context.Categories on pmc.CategoryId equals c.Id into pmcc
                        from c in ppmc.DefaultIfEmpty()
                        join pimg in _context.ProductImages on p.Id equals pimg.ProductId
                        into ppimg
                        from pimg in ppimg.DefaultIfEmpty()
                        select new { p, pmc, c, pimg };
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.p.ProductName.Contains(request.Keyword));
            }
            if (request.CategoryIds.Count > 0)
            {
                query = query.Where(p => request.CategoryIds.Contains(p.pmc.CategoryId));
            }
            //3. paging
            //ham skip lay data tiep theo vd trang 1 (1-1 * 5) = 0 lay 5 sp tiep theo la den sp thu 1 den 5
            //                              trang 2 (2-1 *5) = 5 lay 5 sp tiep theo tu sp 6 toi 10
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    ProductName = x.p.ProductName,
                    BrandId = x.p.BrandId,
                    Description = x.p.Description,
                    Installment = x.p.Installment,
                    PriceCurrent = x.p.PriceCurrent,
                    PriceOld = x.p.PriceOld,
                    SaleOff = x.p.SaleOff,
                    Slug = x.p.Slug,
                    SoldOut = x.p.SoldOut,
                    CategoryName = x.c.Category.NameCategory,
                    ThumbnailImage = x.pimg.Path
                })
                .ToListAsync();

            //4. select and projection

            var pagedResult = new PageResult<ProductViewModel>()
            {
                Items = data,
                TotalRecord = totalRow
            };
            return pagedResult;

        }


        public async Task<bool> UpdatePriceCurrent(string productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new CollabException($"Cannot find product with id: {productId}");
            }
            product.PriceCurrent = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePriceOld(string productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new CollabException($"Cannot find product with id: {productId}");
            }
            product.PriceOld = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateSaleOff(string productId, int newSaleOff)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new CollabException($"Cannot find product with id: {productId}");
            }
            product.SaleOff = newSaleOff;
            return await _context.SaveChangesAsync() > 0;
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }


        //method get product images by product id
        public async Task<List<ProductImageViewModel>> GetListImage(string productId)
        {
            var listProductImages = await _context.ProductImages.Where(x => x.ProductId.Equals(productId)).Select(i => new ProductImageViewModel()
            {
                Id = i.Id,
                Alt = i.Alt,
                Path = i.Path,
            }).ToListAsync();
            return listProductImages;
        }
        public async Task<string> AddImages(string productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Id = request.Id,
                ProductId = productId,
                // Path = request.Path,
                Alt = request.Alt
            };
            if (request.File != null)
            {
                productImage.Path = await this.SaveFile(request.File);
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;

        }

        //support delete file
        private async Task DeleteFile(string fileName)
        {
            // var pathImage = Path.Combine(_userContentFolder, fileName);
            if (fileName == null)
            {
                throw new CollabException($"Cannot find file with path {fileName}");
            }
            await _storageService.DeleteFileAsync(fileName);

        }
        //method remove file
        public async Task<int> RemoveImage(string imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
            {
                throw new CollabException($"Product Image with image id : {productImage.Id} not exists!!!");
            }
            _context.ProductImages.Remove(productImage);
            string fullPath = "wwwroot" + productImage.Path;
            if (File.Exists(fullPath))
            {
                await Task.Run(() =>
                {
                    File.Delete(fullPath);
                });
            }
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateImage(string imageId, ProductImageEditRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
            {
                throw new CollabException($"Cannot find product image with id: {productImage.Id}");
            }
            if (request.File != null)
            {
                string fullPath = "wwwroot" + productImage.Path;
                File.Delete(fullPath);
                productImage.Path = await this.SaveFile(request.File);
                productImage.Alt = request.Alt;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<ProductImageViewModel> GetProductImageById(string imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
            {
                throw new CollabException($"Cannot find ImageProduct with id: {productImage.Id}");
            }
            var viewModel = new ProductImageViewModel()
            {
                Id = productImage.Id,
                Alt = productImage.Alt,
                Path = productImage.Path,
            };
            return viewModel;
        }
    }
}

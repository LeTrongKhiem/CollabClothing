using System.Net.Mime;
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
using Microsoft.AspNetCore.Mvc.Rendering;
using CollabClothing.ViewModels.Catalog.Sizes;
using CollabClothing.ViewModels.Catalog.Color;

namespace CollabClothing.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly CollabClothingDBContext _context;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";
        private readonly IUtilities _utilitiesHelp;
        public ManageProductService(CollabClothingDBContext context, IStorageService storageService, IUtilities utilitiesHelp)
        {
            _context = context;
            _storageService = storageService;
            _utilitiesHelp = utilitiesHelp;
        }


        // public async Task AddViewCount(string productId)
        // {
        //     var product = await _context.Products.FindAsync(productId);
        //     product.ViewCount += 1;
        //     await _context.SaveChangesAsync();
        // }
        //create product ProductCreateRequest la ham duoc tao ben CollabClothing.ViewModels dung de the hien cac thuoc tinh maf nguoi dung co the nhap 
        //de tao nen 1 san pham
        #region Save/Delete File
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        //support delete file
        private async Task DeleteFile(string fileName)
        {
            if (fileName == null)
            {
                throw new CollabException($"Cannot find file with path {fileName}");
            }
            await _storageService.DeleteFileAsync(fileName);

        }
        #endregion
        #region Create Product
        public async Task<string> Create(ProductCreateRequest request)
        {
            Guid g = Guid.NewGuid();
            List<ProductMapCategory> cates = new List<ProductMapCategory>();
            foreach (var item in request.CategoryId)
            {
                cates.Add(new ProductMapCategory()
                {
                    ProductId = g.ToString(),
                    CategoryId = item.ToString()
                });
                //_context.ProductMapCategories.Add(ProductMapCategory);

            }
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
                Slug = _utilitiesHelp.SEOUrl(request.ProductName),
                Details = request.Details,
                ProductMapCategories = cates

            };
            //var ProductMapCategory = new ProductMapCategory()
            //{
            //    ProductId = product.Id,
            //    CategoryId = request.CategoryId
            //};
            Guid productDetailsId = Guid.NewGuid();
            var productDetails = new ProductDetail()
            {
                Id = productDetailsId.ToString(),
                ProductId = product.Id,
                Consumer = request.Consumer,
                Cotton = request.Cotton,
                Form = request.Form,
                Type = request.Type,
                MadeIn = request.MadeIn
            };
            foreach (var item in request.ThumbnailImage)
            {
                Guid g2 = Guid.NewGuid();
                var Thumbnail = new ProductImage()
                {
                    Id = g2.ToString(),
                    ProductId = product.Id,
                    Alt = product.ProductName,
                    IsThumbnail = true


                };
                if (request.ThumbnailImage != null)
                {
                    Thumbnail.Path = await this.SaveFile(item);
                }
                else
                {
                    Thumbnail.Path = "no-image";
                }
                _context.ProductImages.Add(Thumbnail);
            }

            _context.Products.Add(product);
            //_context.ProductMapCategories.Add(ProductMapCategory);
            _context.ProductDetails.Add(productDetails);

            await _context.SaveChangesAsync();
            return product.Id;
        }
        #endregion
        //assign category
        public async Task<bool> CategoryAssign(string id, CategoryAssignRequest request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }
            foreach (var category in request.Categories)
            {
                var productMapCate = await _context.ProductMapCategories.FirstOrDefaultAsync(x => x.CategoryId == category.Id
                        && x.ProductId == product.Id);
                if (productMapCate != null && category.Selected == false)
                {
                    _context.ProductMapCategories.Remove(productMapCate);
                }
                else if (productMapCate == null && category.Selected == true)
                {
                    await _context.ProductMapCategories.AddAsync(new ProductMapCategory()
                    {
                        CategoryId = category.Id,
                        ProductId = id
                    });
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> PromotionAssign(string id, PromotionAssignRequest request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }
            foreach (var promotion in request.Promotions)
            {
                var promotionMap = await _context.Promotions.FirstOrDefaultAsync(x => x.Id == promotion.Id && x.ProductId == product.Id);
                if (promotionMap != null && promotion.Selected == false)
                {
                    _context.Promotions.Remove(promotionMap);
                }
                else if (promotionMap == null && promotion.Selected == true)
                {
                    await _context.Promotions.AddAsync(new Promotion()
                    {
                        Id = promotion.Id,
                        ProductId = id
                    });
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }
        #region Color Assign
        public async Task<bool> ColorAssign(string id, ColorAssignRequest request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }
            foreach (var color in request.Colors)
            {
                var colorMap = await _context.ProductMapColors.FirstOrDefaultAsync(x => x.ColorId == color.Id && x.ProductId == product.Id);
                if (colorMap != null && color.Selected == false)
                {
                    _context.ProductMapColors.Remove(colorMap);
                }
                else if (colorMap == null && color.Selected == true)
                {
                    await _context.ProductMapColors.AddAsync(new ProductMapColor()
                    {
                        ProductId = id,
                        ColorId = color.Id
                    });
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
        #region Size Assign
        public async Task<bool> SizeAssign(string id, SizeAssignRequest request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }
            foreach (var size in request.Sizes)
            {
                var productMapSize = await _context.ProductMapSizes.FirstOrDefaultAsync(x => x.SizeId == size.Id && x.ProductId == product.Id);
                if (productMapSize != null && size.Selected == false)
                {
                    _context.ProductMapSizes.Remove(productMapSize);
                }
                else if (productMapSize == null && size.Selected == true)
                {
                    await _context.ProductMapSizes.AddAsync(new ProductMapSize()
                    {
                        ProductId = id,
                        SizeId = size.Id
                    });
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
        //method dung de delete product khai bao bien product dung de tim kiem product bang id
        //va bien image tim cac hinh anh cos ma san pham tuong ung duyet qua va xoa
        public async Task<int> Delete(string productId)
        {
            var productMapCate = await _context.ProductMapCategories.Where(x => x.ProductId == productId).ToListAsync();
            var product = await _context.Products.FindAsync(productId);
            var productImage = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductId == productId);
            var productDetail = await _context.ProductDetails.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (product.Id == null)
                throw new CollabException($"Cannot find a product: {productId}");
            var images = _context.ProductImages.Where(i => i.ProductId == productId);

            //foreach (var image in images)
            //{
            //    await _storageService.DeleteFileAsync(image.Path);
            //}
            var fullPath = "wwwroot" + productImage.Path;
            if (File.Exists(fullPath))
            {
                await Task.Run(() =>
                {
                    File.Delete(fullPath);
                });
            }
            _context.ProductImages.Remove(productImage);
            foreach (var item in productMapCate)
            {
                _context.ProductMapCategories.Remove(item);
            }
            _context.Products.Remove(product);
            _context.ProductDetails.Remove(productDetail);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(string id, ProductEditRequest request)
        {
            var product = await _context.Products.FindAsync(id);
            var productDetails = await _context.ProductDetails.FirstOrDefaultAsync(x => x.ProductId == id);
            var image = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductId == product.Id);
            if (product == null || productDetails == null)
            {
                throw new CollabException($"Cannot find product with Id: {id}");
            }
            product.ProductName = request.ProductName != null ? request.ProductName : product.ProductName;
            product.Description = request.Description;
            product.BrandId = request.BrandId;
            product.Details = request.Details;
            product.Slug = request.Slug;

            productDetails.Form = request.Form;
            productDetails.Consumer = request.Consumer;
            productDetails.Cotton = request.Cotton;
            productDetails.MadeIn = request.MadeIn;
            productDetails.Type = request.Type;
            if (request.ThumbnailImage == null)
            {
                image.Path = image.Path;
                return await _context.SaveChangesAsync();
            }
            else
            {
                if (image == null)
                {
                    Guid g = Guid.NewGuid();
                    var thumbnail = new ProductImage()
                    {
                        Id = g.ToString(),
                        ProductId = product.Id,
                        Alt = product.ProductName,
                    };
                    if (request.ThumbnailImage != null)
                    {
                        thumbnail.Path = await this.SaveFile(request.ThumbnailImage);
                    }
                    _context.ProductImages.Add(thumbnail);
                    return await _context.SaveChangesAsync();
                }
                else
                {
                    //delete old image file
                    string fullPath = "wwwroot" + image.Path;
                    if (File.Exists(fullPath))
                    {
                        await Task.Run(() =>
                        {
                            File.Delete(fullPath);
                        });
                    }
                    //save image
                    if (request.ThumbnailImage != null)
                    {
                        var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.ProductId == id);

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
            }

        }

        public async Task<ProductViewModel> GetProductById(string productId)
        {
            var product = await _context.Products.FindAsync(productId);
            var categories = await (from c in _context.Categories
                                    join pmc in _context.ProductMapCategories on c.Id equals pmc.CategoryId
                                    where pmc.ProductId == productId
                                    select c.NameCategory).ToListAsync();
            var sizes = await (from size in _context.Sizes
                               join pms in _context.ProductMapSizes on size.Id equals pms.SizeId
                               where pms.ProductId == productId
                               select size.NameSize).ToListAsync();
            var promotions = await (from p in _context.PromotionDetails
                                    join pmp in _context.Promotions on p.Id equals pmp.Id
                                    where pmp.ProductId == productId
                                    select p.NamePromotion).ToListAsync();
            var colors = await (from color in _context.Colors
                                join pmc in _context.ProductMapColors on color.Id equals pmc.ColorId
                                where pmc.ProductId == productId
                                select color.NameColor).ToListAsync();

            var image = await _context.ProductImages.Where(x => x.ProductId == productId).FirstOrDefaultAsync();
            var productDetails = await _context.ProductDetails?.FirstOrDefaultAsync(x => x.ProductId == productId);
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
                Description = product?.Description,
                BrandId = product.BrandId,
                Installment = product.Installment,
                Slug = product.Slug,
                SoldOut = product.SoldOut,
                Categories = categories,
                ThumbnailImage = image != null ? image.Path : "no-image.jpg",
                Details = product?.Details,
                Consumer = productDetails.Consumer,
                Type = productDetails?.Type,
                Cotton = (bool)(productDetails?.Cotton),
                Form = productDetails?.Form,
                MadeIn = productDetails?.MadeIn,
                Sizes = sizes,
                Promotions = promotions,
                Colors = colors
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
            var cateBySlug = await _context.Categories.FirstOrDefaultAsync(x => x.Slug == request.Slug);
            //1. Select join
            var query = (from p in _context.Products
                         join pmc in _context.ProductMapCategories on p.Id equals pmc.ProductId into ppmc
                         from pmc in ppmc.DefaultIfEmpty()
                         join c in _context.Categories on pmc.CategoryId equals c.Id into pmcc
                         from c in pmcc.DefaultIfEmpty()
                         join pimg in _context.ProductImages on p.Id equals pimg.ProductId into ppimg
                         from pimg in ppimg.DefaultIfEmpty()
                         join b in _context.Brands on p.BrandId equals b.Id into pb
                         from b in pb.DefaultIfEmpty()
                         where (pimg.IsThumbnail == true)
                         //orderby p.Id descending
                         //orderby p.PriceCurrent ascending
                         select new { p, pmc, c, pimg, b });
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.p.ProductName.Contains(request.Keyword) || x.b.NameBrand.Contains(request.Keyword));
            }
            if (!string.IsNullOrEmpty(request.Slug) && !request.Slug.Equals("all"))
            {
                query = query.Where(x => x.c.Slug == request.Slug || x.c.ParentId == cateBySlug.Id);
            }
            if (!string.IsNullOrEmpty(request.CategoryId) && !request.CategoryId.Equals("all"))
            {
                query = query.Where(x => x.pmc.CategoryId == request.CategoryId || x.c.ParentId == request.CategoryId);
            }
            if (!string.IsNullOrEmpty(request.BrandId))
            {
                query = query.Where(x => x.p.BrandId == request.BrandId);
            }
            if (request.Price != null)
            {
                if (request.Price.Equals("cao-den-thap"))
                {
                    query = query.OrderByDescending(x => x.p.PriceCurrent);
                }
                else
                {
                    query = query.OrderBy(x => x.p.PriceCurrent);
                }
            }
            //3. paging
            //ham skip lay data tiep theo vd trang 1 (1-1 * 5) = 0 lay 5 sp tiep theo la den sp thu 1 den 5
            //                              trang 2 (2-1 *5) = 5 lay 5 sp tiep theo tu sp 6 toi 10
            if (query == null)
            {
                return null;
            }
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    ProductName = x.p.ProductName,
                    BrandId = x.b.NameBrand,
                    Description = x.p.Description,
                    Installment = x.p.Installment,
                    PriceCurrent = x.p.PriceCurrent,
                    PriceOld = x.p.PriceOld,
                    SaleOff = x.p.SaleOff,
                    Slug = x.p.Slug,
                    SoldOut = x.p.SoldOut,
                    CategoryName = x.c.NameCategory,
                    ThumbnailImage = x.pimg.Path,
                    BrandName = x.b.NameBrand
                })
                .ToListAsync();
            //4. select and projection
            var pagedResult = new PageResult<ProductViewModel>()
            {
                Items = data,
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            return pagedResult;

        }

        public async Task<PageResult<ProductViewModel>> GetAllPaging(GetManageProductRequestPaging request, string priceOrder)
        {
            var cateBySlug = await _context.Categories.FirstOrDefaultAsync(x => x.Slug == request.Slug);
            //1. Select join
            var query = (from p in _context.Products
                         join pd in _context.ProductDetails on p.Id equals pd.ProductId into ppd
                         from pd in ppd.DefaultIfEmpty()
                         join pmc in _context.ProductMapCategories on p.Id equals pmc.ProductId into ppmc
                         from pmc in ppmc.DefaultIfEmpty()
                         join c in _context.Categories on pmc.CategoryId equals c.Id into pmcc
                         from c in pmcc.DefaultIfEmpty()
                         join pimg in _context.ProductImages on p.Id equals pimg.ProductId into ppimg
                         from pimg in ppimg.DefaultIfEmpty()
                         join b in _context.Brands on p.BrandId equals b.Id into pb
                         from b in pb.DefaultIfEmpty()
                         where (pimg.IsThumbnail == true)
                         orderby p.PriceOld descending
                         //orderby p.PriceCurrent ascending
                         select new { p, pmc, c, pimg, b, pd });
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.p.ProductName.Contains(request.Keyword) || x.b.NameBrand.Contains(request.Keyword));
            }
            if (!string.IsNullOrEmpty(request.Slug) && !request.Slug.Equals("all"))
            {
                query = query.Where(x => x.c.Slug == request.Slug || x.c.ParentId == cateBySlug.Id);
            }
            if (!string.IsNullOrEmpty(request.CategoryId) && !request.CategoryId.Equals("all"))
            {
                query = query.Where(x => x.pmc.CategoryId == request.CategoryId || x.c.ParentId == request.CategoryId);
            }
            if (!string.IsNullOrEmpty(request.BrandId))
            {
                query = query.Where(x => x.p.BrandId == request.BrandId);
            }
            if (priceOrder != null)
            {
                if (priceOrder.Equals("cao-den-thap"))
                {
                    query = query.OrderByDescending(x => x.p.PriceCurrent);
                }
                else
                {
                    query = query.OrderBy(x => x.p.PriceCurrent);
                }
            }
            //3. paging
            //ham skip lay data tiep theo vd trang 1 (1-1 * 5) = 0 lay 5 sp tiep theo la den sp thu 1 den 5
            //                              trang 2 (2-1 *5) = 5 lay 5 sp tiep theo tu sp 6 toi 10
            if (query == null)
            {
                return null;
            }
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    ProductName = x.p.ProductName,
                    BrandId = x.b.NameBrand,
                    Description = x.p.Description,
                    Installment = x.p.Installment,
                    PriceCurrent = x.p.PriceCurrent,
                    PriceOld = x.p.PriceOld,
                    SaleOff = x.p.SaleOff,
                    Slug = x.p.Slug,
                    SoldOut = x.p.SoldOut,
                    CategoryName = x.c.NameCategory,
                    ThumbnailImage = x.pimg.Path,
                    BrandName = x.b.NameBrand
                })
                .ToListAsync();
            //4. select and projection
            var pagedResult = new PageResult<ProductViewModel>()
            {
                Items = data,
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
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



        //method get product images by product id
        public async Task<List<ProductImageViewModel>> GetListImage(string productId)
        {
            var listProductImages = await _context.ProductImages.Where(x => x.ProductId.Equals(productId)).Select(i => new ProductImageViewModel()
            {
                Id = i.Id,
                Alt = i.Alt,
                Path = i.Path,
                IsThumbnail = i.IsThumbnail,
                productId = productId
            }).ToListAsync();
            return listProductImages;
        }
        public async Task<string> AddImages(string productId, ProductImageCreateRequest request)
        {
            Guid g = Guid.NewGuid();
            var productImage = new ProductImage()
            {
                Id = g.ToString(),
                ProductId = productId,
                Alt = request.Alt,
                IsThumbnail = request.IsThumbnail
            };
            foreach (var item in request.File)
            {
                if (request.File != null)
                {
                    productImage.Path = await this.SaveFile(item);
                }
                _context.ProductImages.Add(productImage);

            }
            await _context.SaveChangesAsync();
            return productImage.Id;
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
                productImage.Id = productImage.Id;
                productImage.Path = await this.SaveFile(request.File);
                productImage.Alt = request.Alt != null ? request.Alt : productImage.Alt;
                productImage.IsThumbnail = request.IsThumbnail;
                _context.ProductImages.Update(productImage);
                return await _context.SaveChangesAsync();
            }
            else
            {
                productImage.Id = productImage.Id;
                productImage.Path = productImage.Path;
                productImage.Alt = request.Alt != null ? request.Alt : productImage.Alt;
                productImage.IsThumbnail = request.IsThumbnail;
                _context.ProductImages.Update(productImage);
                return await _context.SaveChangesAsync();
            }
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
                IsThumbnail = productImage.IsThumbnail
            };
            return viewModel;
        }

        public async Task<List<ProductViewModel>> GetFeaturedProducts(int take)
        {

            var query = (from p in _context.Products
                         join pimg in _context.ProductImages on p.Id equals pimg.ProductId
                         join b in _context.Brands on p.BrandId equals b.Id
                         into pb
                         from b in pb.DefaultIfEmpty()
                         where pimg.IsThumbnail == true
                         select new { p, pimg, b });
            List<ProductViewModel> data = await query.Take(take).OrderBy(x => x.p.PriceCurrent)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    ProductName = x.p.ProductName,
                    BrandId = x.b.NameBrand,
                    Description = x.p.Description,
                    Installment = x.p.Installment,
                    PriceCurrent = x.p.PriceCurrent,
                    PriceOld = x.p.PriceOld,
                    SaleOff = x.p.SaleOff,
                    Slug = x.p.Slug,
                    SoldOut = x.p.SoldOut,
                    //CategoryName = x.c.NameCategory,
                    ThumbnailImage = x.pimg.Path,
                    BrandName = x.b.NameBrand
                })
            .ToListAsync();
            return data;
        }

        public async Task<List<ProductViewModel>> GetFeaturedProductsCategory(string idCate, int take)
        {

            var query = (from p in _context.Products
                         join cateMap in _context.ProductMapCategories on p.Id equals cateMap.ProductId
                         into pcateMap
                         from cateMap in pcateMap.DefaultIfEmpty()
                         join cate in _context.Categories on cateMap.CategoryId equals cate.Id
                         into cateMapcate
                         from cate
                         in cateMapcate.DefaultIfEmpty()
                         join pimg in _context.ProductImages on p.Id equals pimg.ProductId
                         join b in _context.Brands on p.BrandId equals b.Id
                         into pb
                         from b in pb.DefaultIfEmpty()

                         where (pimg.IsThumbnail == true) && (cate.ParentId.Equals(idCate) || cate.Id.Equals(idCate))

                         select new { p, pimg, b });
            List<ProductViewModel> data = await query.Take(take).OrderBy(x => x.p.PriceCurrent)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    ProductName = x.p.ProductName,
                    BrandId = x.b.NameBrand,
                    Description = x.p.Description,
                    Installment = x.p.Installment,
                    PriceCurrent = x.p.PriceCurrent,
                    PriceOld = x.p.PriceOld,
                    SaleOff = x.p.SaleOff,
                    Slug = x.p.Slug,
                    SoldOut = x.p.SoldOut,
                    //CategoryName = x.c.NameCategory,
                    ThumbnailImage = x.pimg.Path,
                    BrandName = x.b.NameBrand
                })
            .ToListAsync();
            return data;
        }

        public async Task<PageResult<ProductViewModel>> GetProductByCategory(GetPublicProductRequestPaging request)
        {
            var query = (from p in _context.Products
                         join pmc in _context.ProductMapCategories on p.Id equals pmc.ProductId
                         into ppmc
                         from pmc in ppmc.DefaultIfEmpty()
                         join c in _context.Categories on pmc.CategoryId equals c.Id
                         into pmcc
                         from c in pmcc.DefaultIfEmpty()
                         join pimg in _context.ProductImages on p.Id equals pimg.ProductId
                         //into ppimg
                         //from pimg in ppimg.DefaultIfEmpty()
                         join b in _context.Brands on p.BrandId equals b.Id
                         into pb
                         from b in pb.DefaultIfEmpty()
                         select new { p, pimg, b, c });
            if (request.CategoryId != null)
            {
                query = query.Where(x => x.c.Id == request.CategoryId);
            }
            int totalRow = await query.CountAsync();
            var data = await query.Skip(request.PageSize * (request.PageIndex - 1))
                                    .Take(request.PageSize)
                                    .Select(x => new ProductViewModel()
                                    {
                                        Id = x.p.Id,
                                        ProductName = x.p.ProductName,
                                        BrandId = x.b.NameBrand,
                                        Description = x.p.Description,
                                        Installment = x.p.Installment,
                                        PriceCurrent = x.p.PriceCurrent,
                                        PriceOld = x.p.PriceOld,
                                        SaleOff = x.p.SaleOff,
                                        Slug = x.p.Slug,
                                        SoldOut = x.p.SoldOut,
                                        //CategoryName = x.c.NameCategory,
                                        ThumbnailImage = x.pimg.Path,
                                        BrandName = x.b.NameBrand
                                    }).ToListAsync();
            var pageResult = new PageResult<ProductViewModel>()
            {
                Items = data,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                TotalRecord = totalRow
            };
            return pageResult;
        }

        public async Task<List<ProductViewModel>> GetRelatedProduct(string productId, int take)
        {
            var query = (from p in _context.Products
                         join pmc in _context.ProductMapCategories
                         on p.Id equals pmc.ProductId
                         into ppmc
                         from pmc in ppmc.DefaultIfEmpty()
                         join c in _context.Categories
                         on pmc.CategoryId equals c.Id
                         into pmcc
                         from c in pmcc.DefaultIfEmpty()
                         join pimg in _context.ProductImages on p.Id equals pimg.ProductId
                         join b in _context.Brands on p.BrandId equals b.Id
                         into pb
                         from b in pb.DefaultIfEmpty()
                         where pimg.IsThumbnail == true && p.Id == productId
                         select new { p, pimg, b, c });
            List<ProductViewModel> data = await query.Take(take).OrderBy(x => x.p.PriceCurrent)
               .Select(x => new ProductViewModel()
               {
                   Id = x.p.Id,
                   ProductName = x.p.ProductName,
                   BrandId = x.b.NameBrand,
                   Description = x.p.Description,
                   Installment = x.p.Installment,
                   PriceCurrent = x.p.PriceCurrent,
                   PriceOld = x.p.PriceOld,
                   SaleOff = x.p.SaleOff,
                   Slug = x.p.Slug,
                   SoldOut = x.p.SoldOut,
                   CategoryName = x.c.NameCategory,
                   ThumbnailImage = x.pimg.Path,
                   BrandName = x.b.NameBrand
               })
           .ToListAsync();
            return data;
        }

        public async Task<PageResult<ProductViewModel>> GetProductLoadMore(int amount, string cateId)
        {
            throw new Exception();
        }

        public string GetBrandByProductId(string productId)
        {
            var query = from p in _context.Products
                        join b in _context.Brands on p.BrandId equals b.Id
                        into pb
                        from b in pb.DefaultIfEmpty()
                        where p.Id == productId
                        select new { p, b };
            var result = query.Select(x => x.b.NameBrand).FirstOrDefault();
            return result;
        }

        public List<SizeViewModel> GetNameSize(string productId)
        {
            var query = from s in _context.Sizes
                        join pms in _context.ProductMapSizes on s.Id equals pms.SizeId
                        into pmss
                        from pms in pmss.DefaultIfEmpty()
                        where pms.ProductId == productId
                        select new { s, pms };
            var result = query.Select(x => new SizeViewModel()
            {
                Id = x.s.Id,
                Name = x.s.NameSize
            }).ToList();
            return result;
        }

        public List<ColorViewModel> GetColorSize(string productId)
        {
            var query = from c in _context.Colors
                        join pmc in _context.ProductMapColors on c.Id equals pmc.ColorId
                        into pmcc
                        from pmc in pmcc.DefaultIfEmpty()
                        where pmc.ProductId == productId
                        select new { c, pmc };
            var result = query.Select(x => new ColorViewModel()
            {
                Id = x.c.Id,
                Name = x.c.NameColor
            }).ToList();
            return result;
        }

        public async Task<string> GetNameProductById(string id)
        {
            var query = (from p in _context.Products where p.Id == id select p.ProductName).FirstOrDefault();
            return query;
        }

        public async Task<int> GetQuantityRemain(string productId)
        {
            var query = from w in _context.WareHouses
                        join p in _context.Products
                        on w.ProductId equals p.Id
                        where w.ProductId == productId
                        select w;
            var wareHouse = await query.Select(x => x.Quantity).FirstOrDefaultAsync();
            return wareHouse;
        }

        public async Task<int> GetQuantityRemain(string productId, string sizeId, string colorId)
        {
            var query = from w in _context.WareHouses
                        join p in _context.Products
                        on w.ProductId equals p.Id
                        where w.ProductId == productId && w.SizeId == sizeId && w.ColorId == colorId
                        select w;
            var wareHouse = await query.Select(x => x.Quantity).FirstOrDefaultAsync();
            return wareHouse;
        }

        public async Task<bool> UpdateQuantityRemainProduct(string productId, WareHouseRequest request)
        {
            var wareHouse = await _context.WareHouses.FirstOrDefaultAsync(x => x.ProductId == request.ProductId && x.SizeId == request.SizeId && x.ColorId == request.ColorId);
            if (wareHouse == null)
            {
                Guid gWareHouse = Guid.NewGuid();
                var createWareHouse = new WareHouse()
                {
                    Id = gWareHouse.ToString(),
                    ProductId = productId,
                    ColorId = request.ColorId,
                    SizeId = request.SizeId,
                    Quantity = request.Quantity
                };
                _context.WareHouses.Add(createWareHouse);
            }
            else
            {
                wareHouse.Quantity = request.Quantity;
            }
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<WareHouseRequest> GetWareHouse(string productId)
        {
            var query = from w in _context.WareHouses
                        join p in _context.Products
                        on w.ProductId equals p.Id
                        where w.ProductId == productId
                        select w;

            var wareHouse = await query.Select(x => new WareHouseRequest()
            {
                Id = x.Id,
                ColorId = x.ColorId,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                SizeId = x.SizeId
            }).FirstOrDefaultAsync();
            if (wareHouse == null)
            {
                return null;
            }
            return wareHouse;
        }

        public async Task<WareHouseRequest> GetWareHouse(string productId, string sizeId)
        {
            var query = from w in _context.WareHouses
                        join p in _context.Products
                        on w.ProductId equals p.Id
                        where w.ProductId == productId
                        select w;
            if (!string.IsNullOrEmpty(sizeId))
            {
                query = query.Where(x => x.SizeId == sizeId);
            }
            var wareHouse = await query.Select(x => new WareHouseRequest()
            {
                Id = x.Id,
                ColorId = x.ColorId,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                SizeId = x.SizeId
            }).FirstOrDefaultAsync();
            return wareHouse;
        }

        public async Task<WareHouseRequest> GetWareHouse(string productId, string sizeId, string colorId)
        {
            var query = from w in _context.WareHouses
                        join p in _context.Products
                        on w.ProductId equals p.Id
                        where w.ProductId == productId
                        select w;
            if (!string.IsNullOrEmpty(sizeId))
            {
                query = query.Where(x => x.SizeId == sizeId);
            }
            if (!string.IsNullOrEmpty(colorId))
            {
                query = query.Where(x => x.ColorId == colorId);
            }
            var wareHouse = await query.Select(x => new WareHouseRequest()
            {
                Id = x.Id,
                ColorId = x.ColorId,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                SizeId = x.SizeId
            }).FirstOrDefaultAsync();
            return wareHouse;
        }
    }
}

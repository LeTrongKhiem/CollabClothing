using CollabClothing.Application.Common;
using CollabClothing.Data.Dtos;
using CollabClothing.Data.EF;
using CollabClothing.Data.Entities;
using CollabClothing.ViewModels.Catalog.Categories;
using CollabClothing.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CollabClothing.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using CollabClothing.Utilities.Exceptions;

namespace CollabClothing.Application.Catalog.Categories
{

    public class CategoryService : ICategoryService
    {
        private readonly CollabClothingDBContext _context;
        private readonly IStorageService _storageService;
        private readonly string USER_CONTENT_FOLDER_NAME = "user-content";
        private readonly string CHILD_PATH_FOLDER_NAME = "icon-category";

        public CategoryService(CollabClothingDBContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName, CHILD_PATH_FOLDER_NAME);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + CHILD_PATH_FOLDER_NAME + "/" + fileName;
        }
        public async Task<string> Create(CategoryCreateRequest request)
        {

            Guid g = Guid.NewGuid();
            var category1 = new CategoryDTO()
            {
                Id = g.ToString(),
                NameCategory = request.CategoryName,
                ParentId = request.ParentId,
                Level = request.Level,
                IsShowWeb = request.IsShowWeb,
                Slug = request.Slug
            };
            if (request.Icon != null)
            {
                category1.Icon = await this.SaveFile(request.Icon);
            }
            else
                category1.Icon = "no-image";
            Category category = new Category();
            category.CateMapping(category1);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category1.Id;
        }
        #region GetAllPaging
        public async Task<ResultApi<PageResult<CategoryViewModel>>> GetAllPaging(GetCategoryRequestPaging request)
        {

            var query = from cate in _context.Categories orderby cate.Level select new { cate };
            var query2 = _context.Categories.Where(x => x.ParentId == x.Id).Select(o => o.NameCategory).ToString();

            var listCateParent = (from cate in _context.Categories orderby cate.Level where cate.ParentId == null select cate.NameCategory).FirstOrDefault();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.cate.NameCategory.Contains(request.Keyword));
            }
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                         .Take(request.PageSize)
                         .Select(x => new CategoryViewModel()
                         {
                             CategoryId = x.cate.Id,
                             CategoryName = x.cate.NameCategory,
                             Icon = x.cate.Icon,
                             IsShowWeb = x.cate.IsShowWeb,
                             Level = x.cate.Level,
                             ParentId = x.cate.ParentId,
                             Slug = x.cate.Slug,
                             //ParentName = query2
                         }).ToListAsync();

            var pageResult = new PageResult<CategoryViewModel>()
            {
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecord = totalRow
            };
            return new ResultApiSuccessed<PageResult<CategoryViewModel>>(pageResult);
        }
        #endregion

        public async Task<CategoryViewModel> GetCateById(string Id)
        {
            var cate = await _context.Categories.FindAsync(Id);
            if (cate == null)
            {
                throw new CollabException($"Not find Category with id: {Id}");
            }
            var cate1 = new CategoryViewModel()
            {
                CategoryId = cate.Id,
                CategoryName = cate.NameCategory,
                ParentId = cate.ParentId,
                IsShowWeb = cate.IsShowWeb,
                Icon = cate.Icon,
                Level = cate.Level,
                Slug = cate.Slug
            };

            Category category = new Category();
            category.CateMapping(new CategoryDTO());
            return cate1;
        }

        public async Task<ResultApi<bool>> Edit(string cateId, CategoryEditRequest request)
        {
            var cate = await _context.Categories.FindAsync(cateId);
            if (cate == null)
            {
                return new ResultApiError<bool>("Mã danh mục không chính xác!!!");
            }
            cate.NameCategory = request.CategoryName;
            cate.IsShowWeb = request.IsShowWeb;
            cate.ParentId = request.ParentId;
            cate.Slug = request.Slug;
            cate.Level = request.Level;

            if (cate.Icon == null)
            {
                if (request.Icon != null)
                {
                    cate.Icon = await this.SaveFile(request.Icon);
                }
                _context.Categories.Add(cate);
                await _context.SaveChangesAsync();
                return new ResultApiSuccessed<bool>();
            }
            else
            {
                if (request.Icon != null)
                {
                    var fullPath = "wwwroot" + cate.Icon;
                    if (File.Exists(fullPath))
                    {
                        await Task.Run(() =>
                        {
                            File.Delete(fullPath);
                        });
                    }
                    cate.Icon = await this.SaveFile(request.Icon);
                    _context.Categories.Update(cate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.Categories.Update(cate);
                    await _context.SaveChangesAsync();
                }
                return new ResultApiSuccessed<bool>();
            }
        }

        public async Task<ResultApi<bool>> Delete(string CateId)
        {
            var cate = await _context.Categories.FindAsync(CateId);
            if (cate.Id == null)
            {
                return new ResultApiError<bool>("Không tìm thấy danh mục sản phẩm");
            }

            var fullPath = "wwwroot" + cate.Icon;
            if (File.Exists(fullPath))
            {
                await Task.Run(() =>
                {
                    File.Delete(fullPath);
                });
            }
            //await _storageService.DeleteFileAsync(cate.Icon);
            _context.Categories.Remove(cate);
            await _context.SaveChangesAsync();
            return new ResultApiSuccessed<bool>();
        }

        public async Task<List<CategoryViewModel>> GetAll()
        {
            var query = from c in _context.Categories orderby c.Level, c.Order select new { c };
            return await query.Select(x => new CategoryViewModel()
            {
                CategoryId = x.c.Id,
                CategoryName = x.c.NameCategory,
                ParentId = x.c.ParentId,
                Icon = x.c.Icon,
                Slug = x.c.Slug
            }).ToListAsync();
        }

        public async Task<List<CategoryViewModel>> GetParentCate()
        {
            var query = from c in _context.Categories where c.ParentId.Equals("null") select new { c };
            return await query.Select(x => new CategoryViewModel()
            {
                CategoryId = x.c.Id,
                CategoryName = x.c.NameCategory,
            })
                .ToListAsync();
        }

        public async Task<List<CategoryViewModel>> GetCateChild(string parentId)
        {
            var query = from c in _context.Categories where c.ParentId.Equals(parentId) select new { c };
            return await query.Select(x => new CategoryViewModel()
            {
                CategoryId = x.c.Id,
                CategoryName = x.c.NameCategory,
                ParentId = x.c.ParentId,
                Icon = x.c.Icon,
                Slug = x.c.Slug
            }).ToListAsync();
        }

        public async Task<string> GetParentNameById(string id)
        {
            var query = (from c in _context.Categories where c.Id == id select c.NameCategory).FirstOrDefault();
            return query;
        }
    }
}

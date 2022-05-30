using CollabClothing.ApiShared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Controllers.Components
{
    public class CategoryChildMenuViewComponent : ViewComponent
    {
        private readonly ICategoryApiClient _categoryApiClient;
        public CategoryChildMenuViewComponent(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync(string parentId)
        {
            var items = await _categoryApiClient.GetCateChild(parentId);
            return View(items);
        }
    }
}

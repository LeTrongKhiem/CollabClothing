
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CollabClothing.ViewModels.Catalog.ProductImages
{
    public class ProductImageCreateRequest
    {
        public string Id { get; set; }
        // public string Product { get; set; }
        // public string Path { get; set; }
        public string Alt { get; set; }
        public List<IFormFile> File { get; set; }
        public bool IsThumbnail { get; set; }

    }
}
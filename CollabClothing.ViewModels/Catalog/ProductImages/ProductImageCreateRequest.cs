
using Microsoft.AspNetCore.Http;

namespace CollabClothing.ViewModels.Catalog.ProductImages
{
    public class ProductImageCreateRequest
    {
        public string Id { get; set; }
        // public string Product { get; set; }
        // public string Path { get; set; }
        public string Alt { get; set; }
        public IFormFile File { get; set; }

    }
}
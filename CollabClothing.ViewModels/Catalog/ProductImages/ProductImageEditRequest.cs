
using Microsoft.AspNetCore.Http;

namespace CollabClothing.ViewModels.Catalog.ProductImages
{
    public class ProductImageEditRequest
    {
        // public string Path { get; set; }
        public string Alt { get; set; }
        // public string ProductId { get; set; }
        public IFormFile File { get; set; }
    }
}
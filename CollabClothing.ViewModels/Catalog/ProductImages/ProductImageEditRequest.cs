
using Microsoft.AspNetCore.Http;

namespace CollabClothing.ViewModels.Catalog.ProductImages
{
    public class ProductImageEditRequest
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public string Alt { get; set; }
        // public string ProductId { get; set; }
        public IFormFile File { get; set; }
        public bool IsThumbnail { get; set; }
    }
}
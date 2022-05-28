using Microsoft.AspNetCore.Http;

namespace CollabClothing.ViewModels.Catalog.ProductImages
{
    public class ProductImageViewModel
    {
        public string Id { get; set; }
        // public string productId { get; set; }
        public string Path { get; set; }
        public string Alt { get; set; }
        public IFormFile ImageFile { get; set; }
        public bool IsThumbnail { get; set; }

    }
}
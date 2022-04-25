using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
namespace CollabClothing.Application.Common
{
    public interface IStorageService
    {
        //get url file name
        string GetFileUrl(string fileName);
        Task SaveFileAsync(Stream mediaBinaryStream, string fileName);
        Task DeleteFileAsync(string fileName);
        Task<string> SaveFile(IFormFile file, string path);
        Task SaveFileAsync(Stream mediaBinaryStream, string fileName, string more);
    }
}
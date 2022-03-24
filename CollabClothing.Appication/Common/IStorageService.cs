using System.IO;
using System.Threading.Tasks;
namespace CollabClothing.Appication.Common
{
    public interface IStorageService
    {
        //get url file name
        string GetFileUrl(string fileName);
        Task SaveFileAsync(Stream mediaBinaryStream, string fileName);
        Task DeleteFileAsync(string fileName);


    }
}
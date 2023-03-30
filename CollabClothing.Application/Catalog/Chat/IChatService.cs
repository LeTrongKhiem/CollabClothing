using CollabClothing.ViewModels.Catalog.Chat;
using CollabClothing.ViewModels.Common;
using System.Threading.Tasks;

namespace CollabClothing.Application.Catalog.Chat
{
    public interface IChatService
    {
        Task<ResultApi<string>> Message(MessageDTO message);
        //Task<ResultApi<string>> SendMessagesToUser(MessageDTO message);
        //Task<ResultApi<string>> SendMessageToGroup(string sender, string receiver, string message);
    }
}

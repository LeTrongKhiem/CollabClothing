using CollabClothing.ViewModels.Catalog.Chat;
using CollabClothing.ViewModels.Common;
using CollabClothing.ViewModels.System;
using Microsoft.Extensions.Options;
using PusherServer;
using System.Threading.Tasks;


namespace CollabClothing.Application.Catalog.Chat
{
    public class ChatService : IChatService
    {
        private readonly IOptions<AppSettings> _appSettings;
        public ChatService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }
        public async Task<ResultApi<string>> Message(MessageDTO message)
        {
            var pusherProp = _appSettings.Value.PusherServer;
            var options = new PusherOptions
            {
                Cluster = pusherProp.Cluster,
                Encrypted = true
            };
            var pusher = new Pusher(
                pusherProp.AppId,
                pusherProp.Key,
                pusherProp.Secret,
                options
            );
            var result = await pusher.TriggerAsync(
                "chat",
                "message",
                new
                {
                    message = message.Message,
                    user = message.UserName
                }
            );
            return new ResultApi<string>()
            {
                IsSuccessed = true,
                Message = "Message sent"
            };
        }
    }
}

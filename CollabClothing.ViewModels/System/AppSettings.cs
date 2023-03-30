namespace CollabClothing.ViewModels.System
{
    public class AppSettings
    {
        public PusherServer PusherServer { get; set; }
    }

    public class PusherServer
    {
        public string AppId { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
        public string Cluster { get; set; }
    }
}

namespace WebApp.Settings
{
    public class SMTPSettings
    {
        public string host { get; set; } = string.Empty;
        public int port { get; set; }
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;

    }
}

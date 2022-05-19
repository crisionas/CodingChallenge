namespace CC.NotificationService.Models
{
    public class SmtpSettings
    {
        public static string SectionName = "SmtpSettings";
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 0;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EnableSsl { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }
}

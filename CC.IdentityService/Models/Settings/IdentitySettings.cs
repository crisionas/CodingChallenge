namespace CC.IdentityService.Models.Settings
{
    public class IdentitySettings
    {
        public static string SectionName = "IdentitySettings";
        public string Secret { get; set; } = string.Empty;
    }
}

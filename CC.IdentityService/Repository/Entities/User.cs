namespace CC.IdentityService.Repository.Entities
{
    public class User
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Company { get; set; }
        public string? Email { get; set; }
        public IList<string> Scopes { get; set; } = new List<string>();
    }
}

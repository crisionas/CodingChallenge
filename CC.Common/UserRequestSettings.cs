using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CC.Common
{
    public class UserRequestSettings : IUserRequestSettings
    {
        private readonly IList<Claim>? _claims;
        public UserRequestSettings(IHttpContextAccessor accessor)
        {
            _claims = accessor.HttpContext?.User.Claims.ToList();
        }

        public string? Username => _claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        public string? Company => _claims?.FirstOrDefault(x => x.Type == "company")?.Value;
        public string? Email => _claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
    }
}

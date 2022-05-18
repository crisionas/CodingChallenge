using CC.Common.Models;

namespace CC.IdentityService.Models.Responses
{
    public class AuthResponse : BaseResponse
    {
        public string? Token { get; set; }
    }
}

using CC.Common.Models;
using CC.IdentityService.Models.Requests;
using CC.IdentityService.Models.Responses;

namespace CC.IdentityService.Interfaces
{
    public interface IAuthWorker
    {
        /// <summary>
        /// Register a user async
        /// </summary>
        /// <param name="request">RegisterRequest</param>
        /// <returns>Task of Base Response</returns>
        Task<BaseResponse> RegistrationAsync(RegisterRequest request);

        /// <summary>
        /// Login a users with username and password
        /// </summary>
        /// <param name="request">AuthRequest</param>
        /// <returns>Task of AuthResponse</returns>
        Task<AuthResponse> AuthenticateAsync(AuthRequest request);
    }
}

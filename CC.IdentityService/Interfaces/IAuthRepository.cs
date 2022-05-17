using CC.IdentityService.Models;

namespace CC.IdentityService.Interfaces
{
    public interface IAuthRepository
    {
        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User</returns>
        Task<User?> GetUserAsync(string username);

        /// <summary>
        /// Save user async
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Task</returns>
        Task SaveUserAsync(User user);
    }
}

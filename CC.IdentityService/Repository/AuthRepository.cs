using CC.IdentityService.Interfaces;
using CC.IdentityService.Models;

namespace CC.IdentityService.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IList<User?> _usersRepo = new List<User?>();

        public async Task<User?> GetUserAsync(string username)
        {
            return await Task.Run(() => _usersRepo.FirstOrDefault(x => x!.Username == username));
        }

        public async Task SaveUserAsync(User user)
        {
            await Task.Run(() => _usersRepo.Add(user));
        }
    }
}

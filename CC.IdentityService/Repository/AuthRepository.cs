using CC.IdentityService.Interfaces;
using CC.IdentityService.Repository.Entities;
using System.Collections.Concurrent;

namespace CC.IdentityService.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IDictionary<string, User> _db = new ConcurrentDictionary<string, User>();

        public async Task<User?> GetUserAsync(string username)
        {
            return await Task.Run(() =>
            {
                _db.TryGetValue(username, out var user);
                return user;
            });
        }

        public async Task SaveUserAsync(User user)
        {
            await Task.Run(() =>
            {
                var entity = _db.TryGetValue(user.Username!, out _);
                if (entity)
                    throw new Exception("The user already exists");
                _db.TryAdd(user.Username!, user);
            });
        }
    }
}

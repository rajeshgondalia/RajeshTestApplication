using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ReqresApi.Client.Configuration;
using ReqresApi.Client.Interfaces;
using ReqresApi.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqresApi.Client.Services
{
    public class ExternalUserService
    {
        private readonly IReqresApiClient _apiClient;
        private readonly IMemoryCache _cache;
        private readonly CacheSettings _cacheSettings;

        public ExternalUserService(
            IReqresApiClient apiClient,
            IMemoryCache cache,
            IOptions<CacheSettings> cacheOptions)
        {
            _apiClient = apiClient;
            _cache = cache;
            _cacheSettings = cacheOptions.Value;
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            string cacheKey = $"User_{userId}";

            if (_cache.TryGetValue(cacheKey, out UserDto cachedUser))
            {
                return cachedUser;
            }

            var user = await _apiClient.GetUserByIdAsync(userId);

            if (user != null)
            {
                _cache.Set(cacheKey, user, TimeSpan.FromSeconds(_cacheSettings.UserCacheDurationSeconds));
            }

            return user;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            const string cacheKey = "AllUsers";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<UserDto> cachedUsers))
            {
                return cachedUsers;
            }

            var users = await _apiClient.GetAllUsersAsync();

            _cache.Set(cacheKey, users, TimeSpan.FromSeconds(_cacheSettings.AllUsersCacheDurationSeconds));
            return users;
        }
    }
}

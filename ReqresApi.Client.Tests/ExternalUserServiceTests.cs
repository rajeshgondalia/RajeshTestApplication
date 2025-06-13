using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using ReqresApi.Client.Configuration;
using ReqresApi.Client.Interfaces;
using ReqresApi.Client.Models;
using ReqresApi.Client.Services;

namespace ReqresApi.Client.Tests
{
    public class ExternalUserServiceTests
    {
        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            // Arrange
            var mockApiClient = new Mock<IReqresApiClient>();
            mockApiClient.Setup(m => m.GetUserByIdAsync(1))
                         .ReturnsAsync(new UserDto { Id = 1, FirstName = "Rajesh" });

            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var cacheSettings = Options.Create(new CacheSettings
            {
                UserCacheDurationSeconds = 60,
                AllUsersCacheDurationSeconds = 120
            });

            var service = new ExternalUserService(mockApiClient.Object, memoryCache, cacheSettings);

            // Act
            var user = await service.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(1, user.Id);
            Assert.Equal("Rajesh", user.FirstName);
        }
    }
}
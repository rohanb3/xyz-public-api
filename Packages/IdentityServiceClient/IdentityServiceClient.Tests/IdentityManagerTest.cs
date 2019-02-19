using NUnit.Framework;
using IdentityServiceClient.Service;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityServiceClient.Tests
{
    public class IdentityManagerTest
    {
        private IIdentityManager _identityManager;

        [SetUp]
        public void Setup()
        {
            
            _identityManager = new IdentityManager(new IdentityServiceClientOptions() { ServiceUrl = "https://localhost:5001/api" }, new MemoryCache(new MemoryCacheOptions()));
        }

        [Test]
        public async Task GetAllUsers_ReturnsNotEmptyPayload()
        {
            var response = await _identityManager.GetAllUsersAsync();
            response = await _identityManager.GetAllUsersAsync();
            response = await _identityManager.GetAllUsersAsync();
            Assert.IsTrue(response.Payload.Count == 3);
        }

        [Test]
        public async Task CheckPermissionTest()
        {
            await _identityManager.CheckPermissionExpiration();
            Assert.IsTrue(_identityManager.CheckPermission("TEST", new string[] { "TEST" }));
        }
    }
}
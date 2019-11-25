using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Api.Tests.Models.User;

namespace Xyzies.TWC.Public.Api.Tests.IntegrationTests.Services
{
    public interface IHttpServiceTest
    {
        Task<TokenModel> GetAuthorizationToken(UserLoginOption userLogin);
        Task<User> CreateNewTestUser(CreateUserModel user, TokenModel token);
        Task DeleteTestUser(Guid userId, TokenModel token);
        Task<User> GetUserProfile(TokenModel token);
    }
}

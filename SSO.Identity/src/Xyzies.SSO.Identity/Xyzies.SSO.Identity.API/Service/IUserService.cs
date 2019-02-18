using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xyzies.SSO.Identity.API.Models.User;

namespace Xyzies.SSO.Identity.API.Service
{
    public interface IUserService
    {
        Task<IEnumerable<Profile>> GetAllUsersAsync();
        Task<Profile> GetUserByIdAsync(string id);
        Task UpdateUserByIdAsync(string id, BaseProfile model);
        Task<Profile> CreateUserAsync(ProfileCreatable model);
        Task DeleteUserByIdAsync(string id);
    }
}

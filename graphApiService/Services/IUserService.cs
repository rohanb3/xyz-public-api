using graphApiService.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace graphApiService.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserProfileDto>> GetAllUsersAsync();
        Task<UserProfileDto> GetUserByIdAsync(string id);
        Task UpdateUserByIdAsync(string id, UserProfileEditableDto userToUpdate);
        Task<UserProfileDto> CreateUserAsync(UserProfileCreatableDto user);
        Task DeleteUserByIdAsync(string id);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using graphApiService.Entities.User;

namespace graphApiService.Services
{
    public interface IUserService
    {
        Task<IEnumerable<ProfileDto>> GetAllUsersAsync();
        Task<ProfileDto> GetUserByIdAsync(string id);
        Task UpdateUserByIdAsync(string id, ProfileEditableDto userToUpdate);
        Task<ProfileDto> CreateUserAsync(ProfileCreatableDto user);
        Task DeleteUserByIdAsync(string id);
    }
}

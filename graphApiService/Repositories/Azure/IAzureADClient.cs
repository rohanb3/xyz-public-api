using System.Collections.Generic;
using System.Threading.Tasks;
using graphApiService.Entities.User;

namespace graphApiService.Repositories.Azure
{
    public interface IAzureAdClient
    {
        Task<UserModel> GetUserById(string id);
        Task<IEnumerable<UserModel>> GetUsers();
        Task PostUser(ProfileCreatableDto user);
        Task PatchUser(string id, ProfileEditableDto user);
        Task DeleteUser(string id);
    }
}

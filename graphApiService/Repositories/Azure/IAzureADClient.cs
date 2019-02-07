using System.Collections.Generic;
using System.Threading.Tasks;
using graphApiService.Entities.User;

namespace graphApiService.Repositories.Azure
{
    public interface IAzureAdClient
    {
        Task<AzureUser> GetUserById(string id);
        Task<IEnumerable<AzureUser>> GetUsers();
        Task PostUser(ProfileCreatable user);
        Task PatchUser(string id, ProfileEditable user);
        Task DeleteUser(string id);
    }
}

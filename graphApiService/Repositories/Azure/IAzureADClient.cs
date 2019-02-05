using graphApiService.Dtos.User;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace graphApiService.Repositories.Azure
{
    public interface IAzureADClient
    {
        Task<UserProfileDto> GetUserById(string objectId);
        Task<IEnumerable<UserProfileDto>> GetUsers();
        Task PostUser(UserProfileCreatableDto user);
        Task PatchUser(string objectId, UserProfileEditableDto user);
        Task DeleteUser(string objectId);
    }
}

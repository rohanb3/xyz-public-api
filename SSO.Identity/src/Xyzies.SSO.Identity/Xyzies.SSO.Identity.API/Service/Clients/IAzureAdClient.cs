using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xyzies.SSO.Identity.API.Models.User;

namespace Xyzies.SSO.Identity.API.Service.Clients
{
    public interface IAzureAdClient
    {
        Task<AzureUser> GetUserById(string id);
        Task<IEnumerable<AzureUser>> GetUsers();
        Task PostUser(ProfileCreatable user);
        Task PatchUser(string id, BaseProfile user);
        Task DeleteUser(string id);
    }
}

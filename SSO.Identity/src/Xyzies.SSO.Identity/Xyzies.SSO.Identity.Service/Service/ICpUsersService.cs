using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xyzies.SSO.Identity.Services.Models.User;

namespace Xyzies.SSO.Identity.Services.Service
{
    public interface ICpUsersService
    {
        Task<List<CpUser>> GetAllCpUsers(string authorRole, string companyId);
        Task<CpUser> GetUserById(int id,int authorId, string authorRole, string companyId);
    }
}

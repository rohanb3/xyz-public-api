using Mapster;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xyzies.SSO.Identity.Data.Helpers;
using Xyzies.SSO.Identity.Data.Repository;
using Xyzies.SSO.Identity.Services.Models.User;

namespace Xyzies.SSO.Identity.Services.Service
{
    public class CpUsersService : ICpUsersService
    {
        private readonly ICpUsersRepository _cpUserRepo;

        public CpUsersService(ICpUsersRepository cpUserRepo)
        {
            _cpUserRepo = cpUserRepo;
        }

        public async Task<List<CpUser>> GetAllCpUsers(string authorRole, int companyId)
        {
            if (authorRole == Consts.Roles.SuperAdmin)
            {
                var users = await _cpUserRepo.GetAsync();
                return users.Adapt<List<CpUser>>();
            }

            if (authorRole == Consts.Roles.RetailerAdmin)
            {
                var companyUsers = await _cpUserRepo.GetByAsync(x => x.CompanyId == companyId);
                return companyUsers.Adapt<List<CpUser>>();
            }
            return null;
        }

        public async Task<CpUser> GetUserById(int id, int authorId, string authorRole, int companyId)
        {
            if (authorId != id && authorRole == Consts.Roles.SalesRep)
            {
                return null;
            }

            if (authorRole == Consts.Roles.SuperAdmin)
            {
                var user = await _cpUserRepo.GetAsync(id);
                return user.Adapt<CpUser>();
            }

            if (authorRole == Consts.Roles.RetailerAdmin || authorRole == Consts.Roles.SalesRep)
            {
                var companyUser = await _cpUserRepo.GetByAsync(x => x.CompanyId == companyId && x.Id == id);
                return companyUser.Adapt<CpUser>();
            }
            return null;
        }
    }
}

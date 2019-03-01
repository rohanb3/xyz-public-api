using Mapster;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<List<CpUser>> GetAllCpUsers()
        {
            var users = await _cpUserRepo.GetAsync();
            return users.Adapt<List<CpUser>>();
        }

        public async Task<CpUser> GetUserById(int id)
        {
            var users = await _cpUserRepo.GetAsync(id);
            return users.Adapt<CpUser>();
        }
    }
}

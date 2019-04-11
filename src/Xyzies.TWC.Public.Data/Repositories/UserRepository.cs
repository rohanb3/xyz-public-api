using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Data.Repositories
{
    public class UserRepository : EfCoreBaseRepository<int, Users>, IUserRepository
    {
        public UserRepository(AppDataContext dbContext)
            : base(dbContext)
        {

        }
    }
}

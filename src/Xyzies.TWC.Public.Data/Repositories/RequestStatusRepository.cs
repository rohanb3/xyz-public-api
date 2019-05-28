using System;
using System.Linq;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Core;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Data.Repositories
{
    public class RequestStatusRepository : EfCoreBaseRepository<Guid, RequestStatus>, IRepository<Guid, RequestStatus>, IRequestStatusRepository
    {
        public RequestStatusRepository(AppDataContext dbContext)
            : base(dbContext)
        {

        }
    }
}

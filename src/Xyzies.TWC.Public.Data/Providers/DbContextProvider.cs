using Microsoft.EntityFrameworkCore;
using Xyzies.TWC.Public.Data.Providers.Behaviour;

namespace Xyzies.TWC.Public.Data.Providers
{
    public class DbContextProvider: AccessPointProvider<DbContext>, IAccessPointProvider<DbContext>
    {
        public DbContextProvider(DbContext context) : base(context) { }
    }
}

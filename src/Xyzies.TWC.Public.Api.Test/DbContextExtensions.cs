﻿using System.Linq;
using Xyzies.TWC.Public.Data;

namespace Xyzies.TWC.Public.Api.Tests
{
    public static class DbContextExtensions
    {
        public static void ClearContext(this CablePortalAppDataContext context)
        {
            var entities = context.ChangeTracker.Entries().Select(x => x.Entity);
            context.RemoveRange(entities);
            context.SaveChanges();
        }
    }
}

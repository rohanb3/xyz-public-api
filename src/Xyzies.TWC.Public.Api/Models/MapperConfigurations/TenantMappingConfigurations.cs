using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Entities.Tenant;

namespace Xyzies.TWC.Public.Api.Models.MapperConfigurations
{
    public static class TenantMappingConfigurations
    {
        public static void ConfigureTenantMappers()
        {
            TypeAdapterConfig<Tenant, TenantModel>.NewConfig()
                .Map(dest => dest.Name, src => src.TenantName);
            TypeAdapterConfig<Tenant, TenantSingleModel>.NewConfig()
                .Map(dest => dest.Name, src => src.TenantName);
            TypeAdapterConfig<TenantRequest, Tenant>.NewConfig()
                .Map(dest => dest.TenantName, src => src.Name);
        }
    }
}

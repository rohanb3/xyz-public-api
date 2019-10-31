using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Entities.ServiceProvider;

namespace Xyzies.TWC.Public.Api.Models.MapperConfigurations
{
    public static class ServiceProviderMappingConfigurations
    {
        public static void ConfigureServiceProviderMappers()
        {
            TypeAdapterConfig<ServiceProvider, ServiceProviderModel>.NewConfig()
                .Map(dest => dest.Name, src => src.ServiceProviderName);
            TypeAdapterConfig<ServiceProvider, ServiceProviderSingleModel>.NewConfig()
                .Map(dest => dest.Name, src => src.ServiceProviderName);
            TypeAdapterConfig<ServiceProviderRequest, ServiceProvider>.NewConfig()
                .Map(dest => dest.ServiceProviderName, src => src.Name);
        }
    }
}

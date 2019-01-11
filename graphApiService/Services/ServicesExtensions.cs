using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace graphApiService.Services
{
    public static class ServicesExtensions
    {
        public static void AddMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}

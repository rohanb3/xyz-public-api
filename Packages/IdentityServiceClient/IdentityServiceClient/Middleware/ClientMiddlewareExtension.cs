using IdentityServiceClient.Service;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServiceClient.Middleware
{
    public static class ClientMiddlewareExtension
    {
        /// <summary>
        /// Needs to setup ServiceApiUri
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityClient(this IServiceCollection services, Action<IdentityServiceClientOptions> setupAction)
        {
            //Let the caller configure us.
            var options = new IdentityServiceClientOptions();
            setupAction(options);

            services.AddSingleton<IdentityServiceClientOptions, IdentityServiceClientOptions>();
            return services.AddSingleton<IIdentityManager>(new IdentityManager(options));
        }
    }
}

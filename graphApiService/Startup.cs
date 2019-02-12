using System;
using graphApiService.Common.Azure;
using graphApiService.Middlewares;
using graphApiService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using graphApiService.Repositories.Azure;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using IdentityServiceClient.Middleware;

namespace graphApiService
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(AzureADB2CDefaults.BearerAuthenticationScheme)
                .AddAzureADB2CBearer(options => Configuration.Bind("AzureAdB2C", options));

            services.AddSwagger();
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings// is it works on deserialize?
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            //services.AddIdentityClient(options =>
            //{
            //    options.ServiceUrl = Configuration["IdentityServiceUri"];
            //});

            services.AddScoped<IAzureAdClient, AzureAdClient>();
            services.AddScoped<IUserService, UserService>();
            services.Configure<AzureAdB2COptions>(Configuration.GetSection("AzureAdB2C"));
            services.Configure<AzureAdGraphApiOptions>(Configuration.GetSection("AzureAdGraphApi"));

            services.AddIdentityClient(options =>
            {
                options.ServiceUrl = Configuration["IdentityServiceUri"];
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseExceptionMiddleware();
                app.UseExceptionHandler();
            }

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseProcessClaims();

            app.UseMvc();
        }
    }
}

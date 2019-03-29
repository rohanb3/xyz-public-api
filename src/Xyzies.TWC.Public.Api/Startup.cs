using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Xyzies.TWC.Public.Api.Managers;
using Xyzies.TWC.Public.Api.Managers.Interfaces;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Data;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Repositories;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Api
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string dbConnectionString = $"Data Source=173.82.28.90;Initial Catalog=TWC02122019;User ID=sa;Password=4@ndr3w.";
            //LOCAL: $"Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = TWC02122019; Integrated Security = True; Pooling = False";
            //REMOTE: $"Data Source=173.82.28.90;Initial Catalog=timewarner_20181026;User ID=sa;Password=4@ndr3w.";
            //RELEASE: Configuration["connectionStrings:db"];
            //$"Data Source=173.82.28.90;Initial Catalog=TWC02122019;User ID=sa;Password=4@ndr3w.";

            services
                //.AddEntityFrameworkSqlServer()
                .AddDbContext<AppDataContext>(ctxOptions =>
            ctxOptions.UseSqlServer(dbConnectionString));
            // ExecutionStrategy

            // Response compression
            // https://docs.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-2.2#brotli-compression-provider
            services.AddResponseCompression(options =>
            {
                // NOTE: Enabling compression on HTTPS connections may expose security problems.
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            services.AddMvc(o => o.EnableEndpointRouting = true).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHealthChecks();
            // TODO: Add check for database connection
            //.AddCheck(new SqlConnectionHealthCheck("MyDatabase", Configuration["ConnectionStrings:DefaultConnection"]));

            services.AddCors(setup => setup
                .AddPolicy("dev", policy =>
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()));

            #region DI configuration
            
            services.AddScoped<DbContext, AppDataContext>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBranchManager, BranchManager>();
            services.AddScoped<ICompanyManager, CompanyManager>();

            #endregion

            services.AddSwaggerGen(options =>
            {
                options.SwaggerGeneratorOptions.IgnoreObsoleteActions = true;

                options.SwaggerDoc("v1", new Info
                {
                    Title = "Public API of XYZies",
                    Version = $"v1.0.0",
                    Description = ""
                });

                options.EnableAnnotations();
                options.DescribeAllEnumsAsStrings();
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                   string.Concat(Assembly.GetExecutingAssembly().GetName().Name, ".xml")));
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts()
                    .UseHttpsRedirection();
            }

            #region Mapper configuration

            TypeAdapterConfig<Branch, BranchModel>.NewConfig();
            TypeAdapterConfig<Company, CompanyModel>.NewConfig();
            TypeAdapterConfig<Branch, UploadBranchModel>.NewConfig();
            TypeAdapterConfig<UploadBranchModel, Branch>.NewConfig();
            TypeAdapterConfig<Company, UploadCompanyModel>.NewConfig();
            TypeAdapterConfig<BranchContact, BranchContactModel>.NewConfig();

            #endregion

            // /api/public-api
            app.UseHealthChecks("/healthz")
                .UseCors("dev")
                .UseResponseCompression()
                .UseMvc()
                .UseSwagger(options => options.RouteTemplate = "swagger/{documentName}/swagger.json")
                .UseSwaggerUI(uiOptions =>
                {
                    uiOptions.SwaggerEndpoint("/api/public-api/swagger/v1/swagger.json", $"v1.0.0");
                    uiOptions.RoutePrefix = "/api/public-api";
                    uiOptions.DisplayRequestDuration();
                });
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

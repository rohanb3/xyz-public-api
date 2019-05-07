﻿using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using HealthChecks.SqlServer;
using HealthChecks.UI.Client;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Swashbuckle.AspNetCore.Swagger;
using Xyzies.TWC.Public.Api.Managers;
using Xyzies.TWC.Public.Api.Managers.Interfaces;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Data;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Repositories;
using Xyzies.TWC.Public.Data.Repositories.Azure;
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

        public static string ServiceBaseUrlPrefix { get; set; } = "/api/public-api/"; // Default

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            string dbConnectionString = Configuration.GetConnectionString("db");
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                StartupException.Throw("Missing the connection string to database");
            }
            services.AddAuthentication(AzureADB2CDefaults.BearerAuthenticationScheme)
                .AddAzureADB2CBearer(options => Configuration.Bind("AzureAdB2C", options));
            // TODO: ExecutionStrategy
            services.AddDbContext<AppDataContext>(ctxOptions =>
                ctxOptions.UseSqlServer(dbConnectionString));

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

            services.AddHealthChecks()
                .AddCheck("SQL-TWC", new SqlServerHealthCheck(dbConnectionString, "SELECT @@VERSION"));

            services.AddCors(setup => setup
                .AddPolicy("dev", policy =>
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()));

            string azureStorageConnectionString = Configuration["connectionStrings:storageAccount"];

            if (!CloudStorageAccount.TryParse(azureStorageConnectionString, out CloudStorageAccount storageAccount))
            {
                throw new ApplicationException("Missing Azure Blob Storage settings");
            }

            services.AddSingleton(storageAccount);

            #region DI configuration

            services.AddScoped<DbContext, AppDataContext>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IAzureCompanyAvatarRepository, AzureCompanyAvatarRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBranchManager, BranchManager>();
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<ICompanyAvatarsManager, CompanyAvatarsManager>();

            #endregion

            services.AddSwaggerGen(options =>
            {
                options.SwaggerGeneratorOptions.IgnoreObsoleteActions = true;

                options.SwaggerDoc("v1", new Info
                {
                    Title = "Public API of XYZies",
                    Version = $"v1.0.0",
                    Description =
                        "Healthcheck End-point: <a href=\"/healthchecks\" target=\"_blank\">/healthchecks</a><br/>" +
                        "Healthcheck UI: <a href=\"/healthchecks-ui\" target=\"_blank\">/healthchecks-ui</a><br/>"
                });

                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Name = "Authorization",
                    Description = "Please enter JWT with Bearer into field",
                    Type = "apiKey"
                });

                options.EnableAnnotations();
                options.DescribeAllEnumsAsStrings();
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                   string.Concat(Assembly.GetExecutingAssembly().GetName().Name, ".xml")));
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            string epApiHealthChecks = $"/healthchecks";

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

            if (env.IsLocal())
            {
                epApiHealthChecks = $"{ServiceBaseUrlPrefix}/healthchecks";
            }

            #region Mapper configuration

            TypeAdapterConfig<Branch, BranchModel>.NewConfig();
            TypeAdapterConfig<Company, CompanyModel>.NewConfig();
            TypeAdapterConfig<Branch, CreateBranchModel>.NewConfig();
            TypeAdapterConfig<CreateBranchModel, Branch>.NewConfig();
            TypeAdapterConfig<Company, CreateCompanyModel>.NewConfig();
            TypeAdapterConfig<BranchContact, BranchContactModel>.NewConfig();

            #endregion

            app.UseHealthChecks(epApiHealthChecks, 8083, new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            })
                .UseHealthChecksUI(options => options.ApiPath = epApiHealthChecks)
                .UseCors("dev")
                .UseResponseCompression()
                .UseAuthentication()
                .UseMvc()
                .UseSwagger(options =>
                {
                    options.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.BasePath = $"{ServiceBaseUrlPrefix}");
                    options.RouteTemplate = "/swagger/{documentName}/swagger.json";
                })
                .UseSwaggerUI(uiOptions =>
                {
                    uiOptions.SwaggerEndpoint("v1/swagger.json", $"v1.0.0");
                    uiOptions.DisplayRequestDuration();
                });
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

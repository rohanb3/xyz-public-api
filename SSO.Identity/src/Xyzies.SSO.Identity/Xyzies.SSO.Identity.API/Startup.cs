﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
using Xyzies.SSO.Identity.Data;
using Xyzies.SSO.Identity.Data.Repository;

namespace Xyzies.SSO.Identity.API
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
            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            services.AddIdentityServer(c =>
            {
                
            });

            // For Azure Custom Identity Provider
            services.AddAuthentication()
                .AddOpenIdConnect("aad_b2c", "SSO", options =>
                {

                });

            string dbConnectionString = "Data Source=.;Initial Catalog=timewarner_20181026;User ID=sa;Password=secret123"; //Configuration["connectionStrings:db"];
            services//.AddEntityFrameworkSqlServer()
                .AddDbContextPool<IdentityDataContext>(ctxOptions => 
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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

            services.AddScoped<DbContext, IdentityDataContext>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            #endregion

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Identity",
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

            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHealthChecks("/healthz")
                .UseHttpsRedirection()
                .UseCors("dev")
                .UseResponseCompression()
                .UseMvc()
                .UseSwagger()
                .UseSwaggerUI(uiOptions =>
                {
                    uiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", $"v1.0.0");
                    uiOptions.DisplayRequestDuration();
                });
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
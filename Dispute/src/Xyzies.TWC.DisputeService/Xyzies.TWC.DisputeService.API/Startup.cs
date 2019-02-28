using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Xyzies.TWC.DisputeService.Data;
using Xyzies.TWC.OptymyzeClient.Client;

namespace Xyzies.TWC.DisputeService.API
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Startup
    {
        private readonly ILogger _logger = null;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
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
            // .AddCheck(new SqlConnectionHealthCheck("MyDatabase", Configuration["ConnectionStrings:DefaultConnection"]));

            services.AddCors(setup => setup
                .AddPolicy("dev", policy =>
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()));

            string dbConnectionString = Configuration.GetConnectionString("db");
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                StartupException.Throw("Missing the connection string to database");
            }

                // "Data Source=.;Initial Catalog=timewarner_20181026;User ID=sa;Password=secret123";
            services.AddEntityFrameworkSqlServer()
                .AddDbContextPool<DisputeDataContext>(ctxOptions =>
                    ctxOptions.UseSqlServer(dbConnectionString));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "TWC.DisputeService",
                    Version = $"v1.0.0",
                    Description = ""
                });

                options.EnableAnnotations();
                options.DescribeAllEnumsAsStrings();
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                    string.Concat(Assembly.GetExecutingAssembly().GetName().Name, ".xml")));
            });

            //services.Configure<OptymyzeOptions>(options => Configuration.GetSection("connectionStrings.optymyze").Bind(options));

            services.AddOptions<OptymyzeOptions>()
                .Bind(Configuration.GetSection("connectionStrings.optymyze"))
                .ValidateDataAnnotations();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
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

            app.UseHealthChecks("/healthz")
                .UseCors("dev")
                .UseResponseCompression()
                .UseMvc()
                .UseSwagger()
                .UseSwaggerUI(uiOptions =>
                {
                    uiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", $"v1.0.0");
                    uiOptions.DisplayRequestDuration();
                });

            _logger.LogDebug("Startup configured successfully.");
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

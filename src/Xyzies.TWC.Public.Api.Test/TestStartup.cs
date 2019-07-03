using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xyzies.TWC.Public.Api.Tests
{
    public class TestStartUp : Startup
    {
        public TestStartUp(IConfiguration configuration, ILogger<Startup> logger) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }
    }
}

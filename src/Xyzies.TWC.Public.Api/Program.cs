using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Xyzies.TWC.Public.Api
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class Program
    {
        public static void Main(string[] args)
        {
            const int port = 8083;

            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel()

                .ConfigureKestrel(serverOptions =>
                {
                    serverOptions.ListenAnyIP(port, listenOptions =>
                    {
                        //listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                        //listenOptions.UseHttps("localhost.pfx", "Secret001");
                    });
                })

                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Debug);
                    logging.AddConsole();
                })
                .Build()
                .Run();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

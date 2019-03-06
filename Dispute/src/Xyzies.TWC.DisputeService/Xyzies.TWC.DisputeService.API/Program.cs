﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Logging;

namespace Xyzies.TWC.DisputeService.API
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Program
    {
        public static void Main(string[] args)
        {
            const int port = 8082;

            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel()
                .ConfigureKestrel(serverOptions =>
                {
                    serverOptions.ListenAnyIP(port, listenOptions =>
                    {
                        // TODO: Setup certificate for HTTPS and HTTP2.0
                        //listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                        //listenOptions.UseHttps("localhost.pfx", "Secret001");
                    });
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .Build()
                .Run();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

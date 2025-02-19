using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;

namespace Products.Common.API.Extension
{
    public static class KestrelExtensions
    {
        public static IWebHostBuilder ConfigureKestrelSettings(this IWebHostBuilder webHostBuilder, IConfiguration configuration)
        {
            webHostBuilder.ConfigureKestrel(serverOptions =>
            {
                var httpPort = configuration.GetValue<int?>("KestrelPorts:Endpoints:Http:Port") ?? 5000;
                serverOptions.ListenAnyIP(httpPort);

                if (IsHttpsAvailable(configuration, out var httpsCertificatePath))
                {
                    var httpsPort = configuration.GetValue<int?>("KestrelPorts:Endpoints:Https:Port") ?? 5001;
                    serverOptions.ListenAnyIP(httpsPort, listenOptions => listenOptions.UseHttps(httpsCertificatePath));
                }

                var grpcPort = configuration.GetValue<int?>("KestrelPorts:Endpoints:gRPC:Port") ?? 5003;
                serverOptions.ListenAnyIP(grpcPort, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
            });

            return webHostBuilder;
        }

        public static IApplicationBuilder ConfigureHttpsRedirection(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (IsHttpsAvailable(configuration, out _))
                app.UseHttpsRedirection();

            return app;
        }

        private static bool IsHttpsAvailable(IConfiguration configuration, out string httpsCertificatePath)
        {
            httpsCertificatePath = configuration["KestrelPorts:Endpoints:Https:CertificatePath"];
            return !string.IsNullOrEmpty(httpsCertificatePath) && File.Exists(httpsCertificatePath);
        }
    }
}

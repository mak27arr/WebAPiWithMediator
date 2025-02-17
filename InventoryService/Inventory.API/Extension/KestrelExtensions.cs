namespace Inventory.API.Extension
{
    //TODO: to package
    internal static class KestrelExtensions
    {
        internal static IWebHostBuilder ConfigureKestrelSettings(this IWebHostBuilder webHostBuilder, IConfiguration configuration)
        {
            webHostBuilder.ConfigureKestrel(serverOptions =>
            {
                var httpPort = configuration.GetValue<int?>("KestrelPorts:Endpoints:Http:Port") ?? 5010;
                serverOptions.ListenAnyIP(httpPort);

                if (IsHttpsAvailable(configuration, out var httpsCertificatePath))
                {
                    var httpsPort = configuration.GetValue<int?>("KestrelPorts:Endpoints:Https:Port") ?? 5011;
                    serverOptions.ListenAnyIP(httpsPort, listenOptions => listenOptions.UseHttps(httpsCertificatePath));
                }
            });

            return webHostBuilder;
        }

        internal static IApplicationBuilder ConfigureHttpsRedirection(this IApplicationBuilder app, IConfiguration configuration)
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

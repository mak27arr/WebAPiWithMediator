namespace WebAPI.API.Extension
{
    public static class KestrelExtensions
    {
        public static IWebHostBuilder ConfigureKestrelSettings(this IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenAnyIP(5000);
                serverOptions.ListenAnyIP(5001, listenOptions => listenOptions.UseHttps());
            });

            return webHostBuilder;
        }
    }
}

using Serilog;

namespace WebAPI.ProductAPI.Extension
{
    internal static class LoggingExtensions
    {
        internal static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            string logDirectory = InitAndGetLogDirPath();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(logDirectory, "log-.txt"), rollingInterval: RollingInterval.Day)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Host.UseSerilog();
        }

        internal static string InitAndGetLogDirPath()
        {
            var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            return logDirectory;
        }
    }
}

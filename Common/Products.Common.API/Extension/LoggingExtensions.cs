using Elastic.Ingest.Elasticsearch;
using Elastic.Serilog.Sinks;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Products.Common.API.Extension
{
    public static class LoggingExtensions
    {
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            string logDirectory = InitAndGetLogDirPath();

            var elasticConfig = builder.Configuration.GetSection("Logging:ElasticSearch");

            var elasticUri = elasticConfig["Uri"];
            var elasticUser = elasticConfig["Username"];
            var elasticPass = elasticConfig["Password"];

            Log.Logger = new LoggerConfiguration()
                .WriteTo.ElasticCloud(endpoint: new Uri(elasticUri), 
                bootstrapMethod: BootstrapMethod.None, 
                username: elasticUser, 
                password: elasticPass)
                .WriteTo.File(Path.Combine(logDirectory, "log-.txt"), rollingInterval: RollingInterval.Day)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Host.UseSerilog();
        }

        private static string InitAndGetLogDirPath()
        {
            var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            return logDirectory;
        }
    }
}

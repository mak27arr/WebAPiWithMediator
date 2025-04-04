using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Ingest.Elasticsearch;
using Elastic.Serilog.Sinks;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Elastic.Transport;
using Microsoft.Extensions.Hosting;
using Products.Common.API.Enricher;

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
            var name = builder.Environment.ApplicationName;
            var hostEnvironment = builder.Environment.IsDevelopment() ? Environments.Development : Environments.Production;

            Log.Logger = new LoggerConfiguration()
                .Enrich.With<SessionIdEnricher>()
                .WriteTo.Elasticsearch(new[] { new Uri(elasticUri) }, opts =>
                {
                    opts.DataStream = new DataStreamName("logs", name, hostEnvironment);
                    opts.BootstrapMethod = BootstrapMethod.Failure;
                }, transport =>
                {
                    transport.Authentication(new BasicAuthentication(elasticUser, elasticPass));
                })
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

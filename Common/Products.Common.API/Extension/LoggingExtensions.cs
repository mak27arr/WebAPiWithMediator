﻿using Microsoft.AspNetCore.Builder;
using Serilog;
using Microsoft.Extensions.Hosting;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;

namespace Products.Common.API.Extension
{
    public static class LoggingExtensions
    {
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            string logDirectory = InitAndGetLogDirPath();

            var elasticConfig = builder.Configuration.GetSection("Logging:ElasticSearch");

            var elasticUriStr = elasticConfig["Uri"];
            var elasticUri = elasticUriStr != null ? new Uri(elasticUriStr) : null;
            var elasticUser = elasticConfig["Username"];
            var elasticPass = elasticConfig["Password"];
            var name = builder.Environment.ApplicationName;
            var hostEnvironment = builder.Environment.IsDevelopment() ? Environments.Development : Environments.Production;

            var elasticSinkOptions = new ElasticsearchSinkOptions(elasticUri)
            {
                AutoRegisterTemplate = true,
                IndexFormat = "logs-{0:yyyy.MM.dd}",
                ModifyConnectionSettings = conn =>
                    conn.BasicAuthentication(elasticUser, elasticPass),
                FailureCallback = (logEvent, ex) => Console.WriteLine($"Elasticsearch log failure: {ex.Message}"),
                EmitEventFailure = EmitEventFailureHandling.WriteToFailureSink | EmitEventFailureHandling.RaiseCallback
            };

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(elasticSinkOptions)
                .WriteTo.File(Path.Combine(logDirectory, "log-.txt"), rollingInterval: RollingInterval.Day)
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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using UserService.Domain.Abstractions;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Configurations;
using UserService.Infrastructure.Mapping;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Repositories;

namespace UserService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMongoDB(configuration);
            services.AddRepositories();
            services.AddAutoMapper(typeof(UserProfileMappingProfile));
            return services;
        }

        private static IServiceCollection AddMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoSection = configuration.GetSection("MongoDB");
            var mongoDbSettings = new MongoDbSettings
            {
                ConnectionString = mongoSection["ConnectionString"] ?? configuration["MONGODB__CONNECTIONSTRING"]!,
                DatabaseName = mongoSection["DatabaseName"] ?? configuration["MONGODB__DATABASENAME"]!
            };

            services.AddSingleton<MongoDbSettings>(sp => mongoDbSettings);
            services.AddSingleton<IMongoClient>(sp =>
            {
                return new MongoClient(mongoDbSettings.ConnectionString);
            });
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}

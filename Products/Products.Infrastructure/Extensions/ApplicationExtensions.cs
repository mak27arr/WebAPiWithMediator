using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Products.Infrastructure.Data;

namespace Products.Infrastructure.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceScope ApplyInfrastructureMaintenanceJobs(this IServiceScope scope)
        {
            using (var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>())
                dbContext.Database.Migrate();

            return scope;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Products.Infrastructure.Extensions;

namespace Products.Core.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceScope ApplyCoreMaintenanceJobs(this IServiceScope scope)
        {
            scope.ApplyInfrastructureMaintenanceJobs();

            return scope;
        }
    }
}

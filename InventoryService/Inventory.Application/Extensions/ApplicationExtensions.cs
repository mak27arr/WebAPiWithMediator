using Inventory.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceScope ApplyApplicationMaintenanceJobs(this IServiceScope scope)
        {
            scope.ApplyInfrastructureMaintenanceJobs();

            return scope;
        }
    }
}

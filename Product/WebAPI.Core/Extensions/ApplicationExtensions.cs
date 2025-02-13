using Microsoft.Extensions.DependencyInjection;
using WebAPI.Infrastructure.Extensions;

namespace WebAPI.Core.Extensions
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

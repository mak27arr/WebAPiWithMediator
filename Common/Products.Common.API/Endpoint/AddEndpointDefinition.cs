using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Products.Common.API.Endpoint
{
    public static class AddEndpointDefinition
    {
        public static IServiceCollection AddEndpointDefinitions(this IServiceCollection services, Type assemblyMarkerType)
        {
            var endpointDefinitionTypes = assemblyMarkerType.Assembly
                .GetTypes()
                .Where(t => typeof(IEndpointDefinition).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in endpointDefinitionTypes)
            {
                services.AddTransient(typeof(IEndpointDefinition), type);
            }

            return services;
        }

        public static WebApplication UseEndpointDefinitions(this WebApplication app)
        {
            var definitions = app.Services.GetServices<IEndpointDefinition>();
            foreach (var definition in definitions)
            {
                definition.RegisterEndpoints(app);
            }

            return app;
        }
    }
}

using Microsoft.AspNetCore.Builder;

namespace Products.Common.API.Endpoint
{
    public interface IEndpointDefinition
    {
        void RegisterEndpoints(WebApplication app);
    }
}
using Microsoft.AspNetCore.Builder;

namespace Inventory.API.Endpoint
{
    public interface IEndpointDefinition
    {
        void RegisterEndpoints(WebApplication app);
    }
}
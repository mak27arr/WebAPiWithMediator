using Asp.Versioning;
using AutoMapper;
using Inventory.Application.DTOs;
using Inventory.Application.Features.Inventory.Commands;
using Inventory.Application.Features.Inventory.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Common.API.Endpoint;

namespace Inventory.API.Endpoint
{
    public class InventoryEndpoints : IEndpointDefinition
    {
        private const string BaseRoute = "/api/v{version:apiVersion}/inventory";

        public void RegisterEndpoints(WebApplication app)
        {
            var versionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1, 0))
                .ReportApiVersions()
                .Build();

            var inventoryGroup = app.MapGroup(BaseRoute)
                .RequireAuthorization(new AuthorizeAttribute { Roles = "Logistics" })
                .WithApiVersionSet(versionSet)
                .HasApiVersion(1.0);

            // POST /api/v1/inventory/add-stock
            inventoryGroup.MapPost("/add-stock", AddStock);
            inventoryGroup.MapPost("/remove-stock", RemoveStock);
            inventoryGroup.MapGet("/products/{productId}/count", ProductCount);
        }

        internal async Task<IResult> AddStock(
            [FromBody] ProductStoreDto productStoreDto,
            IMediator mediator,
            IMapper mapper)
        {
            var command = mapper.Map<AddProductToInventoryCommand>(productStoreDto);
            await mediator.Send(command);
            return Results.Ok("Product added successfully.");
        }

        internal async Task<IResult> RemoveStock(
            [FromBody] ProductStoreDto productStoreDto,
            IMediator mediator,
            IMapper mapper)
        {
            var command = mapper.Map<RemoveProductFromInventoryCommand>(productStoreDto);
            command.ReferenceType = Domain.Events.EventReferenceType.Api;
            command.ReferenceId = string.Empty;
            var result = await mediator.Send(command);
            return Results.Ok($"Product removed successfully. Current count: {result}");
        }

        internal async Task<IResult> ProductCount(int productId, IMediator mediator)
        {
            var query = new GetProductCountQuery { ProductId = productId };
            var count = await mediator.Send(query);
            return count != null ? Results.Ok(count) : Results.NotFound("Product not found.");
        }
    }
}

﻿using Asp.Versioning;
using AutoMapper;
using Inventory.Application.DTOs;
using Inventory.Application.Features.Inventory.Commands;
using Inventory.Application.Features.Inventory.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Common.Auth.Role;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/inventory")]
    [ApiVersion("1.0")]
    [Authorize(Roles = $"{UserRoles.Logistics}")]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public InventoryController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("add-stock")]
        public async Task<IActionResult> AddProduct([FromBody] ProductStoreDto productStoreDto)
        {
            var command = _mapper.Map<AddProductToInventoryCommand>(productStoreDto);
            await _mediator.Send(command);

            return Ok("Product added successfully.");
        }

        [HttpPost("remove-stock")]
        public async Task<IActionResult> RemoveProduct([FromBody] ProductStoreDto productStoreDto)
        {
            var command = _mapper.Map<RemoveProductFromInventoryCommand>(productStoreDto);
            var result = await _mediator.Send(command);

            return Ok($"Product removed successfully. Current count: {result}");
        }

        [HttpGet("products/{productId}/count")]
        public async Task<IActionResult> GetProductCount(int productId)
        {
            var query = new GetProductCountQuery() { ProductId = productId };
            var count = await _mediator.Send(query);

            if (count != null)
                return Ok(count);

            return NotFound("Product not found.");
        }
    }
}

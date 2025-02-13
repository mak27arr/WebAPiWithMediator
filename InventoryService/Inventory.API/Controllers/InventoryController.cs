using AutoMapper;
using Inventory.Application.DTOs;
using Inventory.Application.Features.Inventory.Commands;
using Inventory.Application.Features.Inventory.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public InventoryController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductStoreDto productStoreDto)
        {
            var command = _mapper.Map<AddProductToInventoryCommand>(productStoreDto);  // Map the DTO to the command
            var result = await _mediator.Send(command);

            if (result)
                return Ok("Product added successfully.");

            return BadRequest("Failed to add product.");
        }

        [HttpPost("RemoveProduct")]
        public async Task<IActionResult> RemoveProduct([FromBody] ProductStoreDto productStoreDto)
        {
            var command = _mapper.Map<RemoveProductFromInventoryCommand>(productStoreDto);  // Map the DTO to the command
            var result = await _mediator.Send(command);

            if (result)
                return Ok("Product removed successfully.");

            return BadRequest("Failed to remove product.");
        }

        [HttpGet("GetProductCount/{productId}")]
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

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Core.Commands.Products;
using Products.Core.Queries.ProductQueries;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using Products.Core.DTOs;
using Products.Common.Auth.Role;

namespace Products.ProductAPI.Controllers
{
    [Route("api/v{version:apiVersion}/products")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Manager},{UserRoles.Logistics}")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPaginatedProductsQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            return Ok(await _mediator.Send(query));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery { Id = id });

            if (product == null) 
                return NotFound();

            return Ok(product);
        }

        
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> AddProduct(CreateProductCommand command)
        {
            var product = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductCommand command)
        {
            command.Id = id;
            var updateResult = await _mediator.Send(command);

            return updateResult.Match<IActionResult>(success => NoContent(), notFound => NotFound());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _mediator.Send(new DeleteProductCommand { Id = id });

            return NoContent();
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Core.Commands.Products;
using WebAPI.Core.Queries.ProductQueries;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using WebAPI.Core.DTOs;

namespace WebAPI.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            return Ok(await _mediator.Send(new GetAllProductsQuery()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery { Id = id });

            if (product == null) 
                return NotFound();

            return Ok(product);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> AddProduct(CreateProductCommand command)
        {
            var product = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductCommand command)
        {
            command.Id = id;
            var updateResult = await _mediator.Send(command);

            return updateResult.Match<IActionResult>(success => NoContent(), notFound => NotFound());
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _mediator.Send(new DeleteProductCommand { Id = id });

            return NoContent();
        }
    }
}

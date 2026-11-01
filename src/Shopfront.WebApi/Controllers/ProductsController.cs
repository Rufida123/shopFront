using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopfront.Application.Dtos;
using Shopfront.Application.Features.Products.Commands.ChangeProductPrice;
using Shopfront.Application.Features.Products.Commands.CreateProduct;
using Shopfront.Application.Features.Products.Queries.GetProductById;
using Shopfront.Application.Features.Products.Queries.GetProducts;
using Shopfront.Application.Pagination;
using Shopfront.WebApi.Contracts;

namespace Shopfront.WebApi.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ProductDto>> CreateAsync(
        CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = product.Id }, product);
    }

    [HttpPut("{id:guid}/price")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ProductDto>> ChangePriceAsync(
        Guid id,
        ChangeProductPriceRequest request,
        CancellationToken cancellationToken) =>
        Ok(await sender.Send(new ChangeProductPriceCommand(id, request.Price), cancellationToken));

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        Ok(await sender.Send(new GetProductByIdQuery(id), cancellationToken));

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResult<ProductDto>>> GetAllAsync(
        [FromQuery] GetProductsQuery query,
        CancellationToken cancellationToken) =>
        Ok(await sender.Send(query, cancellationToken));
}

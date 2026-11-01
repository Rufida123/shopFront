using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopfront.Application.Dtos;
using Shopfront.Application.Features.Orders.Commands.PlaceOrder;
using Shopfront.Application.Features.Orders.Queries.GetOrderById;

namespace Shopfront.WebApi.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<OrderDto>> PlaceAsync(
        PlaceOrderCommand command,
        CancellationToken cancellationToken)
    {
        var order = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = order.Id }, order);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        Ok(await sender.Send(new GetOrderByIdQuery(id), cancellationToken));
}

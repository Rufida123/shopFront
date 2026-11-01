using MediatR;
using Shopfront.Application.Dtos;

namespace Shopfront.Application.Features.Orders.Commands.PlaceOrder;

public sealed record PlaceOrderCommand(Guid CustomerId, IReadOnlyList<OrderLineRequest> Lines)
    : IRequest<OrderDto>;

using MediatR;
using Shopfront.Application.Dtos;

namespace Shopfront.Application.Features.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(Guid Id) : IRequest<OrderDto>;

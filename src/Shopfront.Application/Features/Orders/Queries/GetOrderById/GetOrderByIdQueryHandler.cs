using MediatR;
using Shopfront.Application.Abstractions;
using Shopfront.Application.Dtos;
using Shopfront.Application.Exceptions;
using Shopfront.Application.Mappings;

namespace Shopfront.Application.Features.Orders.Queries.GetOrderById;

public sealed class GetOrderByIdQueryHandler(IOrderRepository orders)
    : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await orders.GetByIdReadOnlyAsync(request.Id, cancellationToken)
            ?? throw new OrderNotFoundException(request.Id);

        return order.ToDto();
    }
}

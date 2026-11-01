using MediatR;
using Microsoft.Extensions.Logging;
using Shopfront.Application.Abstractions;
using Shopfront.Application.Dtos;
using Shopfront.Application.Exceptions;
using Shopfront.Application.Mappings;
using Shopfront.Domain.Entities;

namespace Shopfront.Application.Features.Orders.Commands.PlaceOrder;

public sealed class PlaceOrderCommandHandler(
    IProductRepository products,
    IOrderRepository orders,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider,
    ILogger<PlaceOrderCommandHandler> logger) : IRequestHandler<PlaceOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var productIds = request.Lines.Select(line => line.ProductId).ToList();
        var foundProducts = await products.GetByIdsAsync(productIds, cancellationToken);
        var productsById = foundProducts.ToDictionary(product => product.Id);

        var lines = request.Lines
            .Select(line => new OrderLine(FindProduct(productsById, line.ProductId), line.Quantity))
            .ToList();

        var order = Order.Place(request.CustomerId, lines, timeProvider.GetUtcNow());

        await orders.AddAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Order {OrderId} placed for customer {CustomerId}", order.Id, request.CustomerId);

        return order.ToDto();
    }

    private static Product FindProduct(IReadOnlyDictionary<Guid, Product> productsById, Guid productId) =>
        productsById.TryGetValue(productId, out var product)
            ? product
            : throw new ProductNotFoundException(productId);
}

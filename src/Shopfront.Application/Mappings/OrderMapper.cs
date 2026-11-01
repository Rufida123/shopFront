using Shopfront.Application.Dtos;
using Shopfront.Domain.Entities;

namespace Shopfront.Application.Mappings;

public static class OrderMapper
{
    public static OrderDto ToDto(this Order order) => new(
        order.Id,
        order.CustomerId,
        order.PlacedAt,
        order.Items.Select(item => item.ToDto()).ToList(),
        order.Total.ToDto());

    private static OrderItemDto ToDto(this OrderItem item) => new(
        item.ProductId,
        item.ProductName,
        item.UnitPrice.ToDto(),
        item.Quantity,
        item.LineTotal.ToDto());
}

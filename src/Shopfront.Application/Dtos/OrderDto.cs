namespace Shopfront.Application.Dtos;

public sealed record OrderDto(
    Guid Id,
    Guid CustomerId,
    DateTimeOffset PlacedAt,
    IReadOnlyList<OrderItemDto> Items,
    MoneyDto Total);

namespace Shopfront.Application.Dtos;

public sealed record OrderItemDto(
    Guid ProductId,
    string ProductName,
    MoneyDto UnitPrice,
    int Quantity,
    MoneyDto LineTotal);

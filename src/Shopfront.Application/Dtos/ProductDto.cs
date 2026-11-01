namespace Shopfront.Application.Dtos;

public sealed record ProductDto(
    Guid Id,
    string Name,
    string Description,
    MoneyDto Price,
    int StockQuantity);

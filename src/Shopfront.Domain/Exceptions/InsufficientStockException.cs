namespace Shopfront.Domain.Exceptions;

public sealed class InsufficientStockException(string productName, int available)
    : DomainException($"Only {available} units of '{productName}' are in stock");

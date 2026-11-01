namespace Shopfront.Domain.Exceptions;

public sealed class InvalidStockQuantityException(int quantity)
    : DomainException($"Stock quantity cannot be negative but was {quantity}");

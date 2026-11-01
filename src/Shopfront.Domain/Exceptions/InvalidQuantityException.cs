namespace Shopfront.Domain.Exceptions;

public sealed class InvalidQuantityException(int quantity)
    : DomainException($"Quantity must be greater than zero but was {quantity}");

namespace Shopfront.Domain.Exceptions;

public sealed class InvalidPriceException(decimal amount)
    : DomainException($"Price must be greater than zero but was {amount}");

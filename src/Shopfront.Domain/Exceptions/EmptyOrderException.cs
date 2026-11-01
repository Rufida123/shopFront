namespace Shopfront.Domain.Exceptions;

public sealed class EmptyOrderException()
    : DomainException("An order must contain at least one product");

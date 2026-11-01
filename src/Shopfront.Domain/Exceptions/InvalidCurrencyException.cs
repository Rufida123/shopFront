namespace Shopfront.Domain.Exceptions;

public sealed class InvalidCurrencyException(string currency)
    : DomainException($"'{currency}' is not a valid three letter currency code");

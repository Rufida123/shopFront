namespace Shopfront.Domain.Exceptions;

public sealed class CurrencyMismatchException(string expected, string actual)
    : DomainException($"Cannot combine amounts in {expected} and {actual}");

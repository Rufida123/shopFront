using Shopfront.Domain.Exceptions;

namespace Shopfront.Domain.ValueObjects;

public sealed record Money
{
    public const string DefaultCurrency = "USD";
    public const int CurrencyCodeLength = 3;
    public const int DecimalPlaces = 2;

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }

    public string Currency { get; }

    public static Money Of(decimal amount, string currency = DefaultCurrency)
    {
        if (currency.Length != CurrencyCodeLength)
        {
            throw new InvalidCurrencyException(currency);
        }

        var rounded = Math.Round(amount, DecimalPlaces, MidpointRounding.AwayFromZero);
        return new Money(rounded, currency.ToUpperInvariant());
    }

    public static Money Zero(string currency = DefaultCurrency) => Of(0m, currency);

    public Money Add(Money other)
    {
        if (other.Currency != Currency)
        {
            throw new CurrencyMismatchException(Currency, other.Currency);
        }

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Multiply(int factor) => new(Amount * factor, Currency);
}

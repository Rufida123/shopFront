using Shopfront.Domain.Exceptions;
using Shopfront.Domain.ValueObjects;

namespace Shopfront.UnitTests.Domain;

public class MoneyTests
{
    [Fact]
    public void Multiply_AmountByQuantity_ReturnsProduct()
    {
        var unitPrice = Money.Of(4.50m);

        var lineTotal = unitPrice.Multiply(3);

        Assert.Equal(13.50m, lineTotal.Amount);
        Assert.Equal(Money.DefaultCurrency, lineTotal.Currency);
    }

    [Fact]
    public void Add_DifferentCurrencies_ThrowsCurrencyMismatchException()
    {
        var dollars = Money.Of(10m, "USD");
        var euros = Money.Of(10m, "EUR");

        Assert.Throws<CurrencyMismatchException>(() => dollars.Add(euros));
    }

    [Fact]
    public void Of_CurrencyWithWrongLength_ThrowsInvalidCurrencyException()
    {
        Assert.Throws<InvalidCurrencyException>(() => Money.Of(10m, "DOLLAR"));
    }
}

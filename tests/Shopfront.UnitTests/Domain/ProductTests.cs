using Shopfront.Domain.Entities;
using Shopfront.Domain.Exceptions;
using Shopfront.Domain.ValueObjects;

namespace Shopfront.UnitTests.Domain;

public class ProductTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Create_PriceNotAboveZero_ThrowsInvalidPriceException(decimal price)
    {
        Assert.Throws<InvalidPriceException>(
            () => Product.Create("Desk Lamp", "Warm light", Money.Of(price), 10));
    }

    [Fact]
    public void Create_NegativeStock_ThrowsInvalidStockQuantityException()
    {
        Assert.Throws<InvalidStockQuantityException>(
            () => Product.Create("Desk Lamp", "Warm light", Money.Of(25m), -1));
    }

    [Fact]
    public void ChangePrice_ZeroPrice_ThrowsInvalidPriceException()
    {
        var product = SampleProducts.Create();

        Assert.Throws<InvalidPriceException>(() => product.ChangePrice(Money.Of(0m)));
    }

    [Fact]
    public void ChangePrice_PositivePrice_UpdatesPrice()
    {
        var product = SampleProducts.Create(price: 25m);

        product.ChangePrice(Money.Of(30m));

        Assert.Equal(30m, product.Price.Amount);
    }

    [Fact]
    public void DecreaseStock_QuantityAboveStock_ThrowsInsufficientStockException()
    {
        var product = SampleProducts.Create(stock: 3);

        var exception = Assert.Throws<InsufficientStockException>(() => product.DecreaseStock(4));

        Assert.Equal("Only 3 units of 'Desk Lamp' are in stock", exception.Message);
        Assert.Equal(3, product.StockQuantity);
    }

    [Fact]
    public void DecreaseStock_QuantityWithinStock_ReducesStock()
    {
        var product = SampleProducts.Create(stock: 10);

        product.DecreaseStock(4);

        Assert.Equal(6, product.StockQuantity);
    }

    [Fact]
    public void DecreaseStock_AllUnits_LeavesStockAtZero()
    {
        var product = SampleProducts.Create(stock: 2);

        product.DecreaseStock(2);

        Assert.Equal(0, product.StockQuantity);
    }

    [Fact]
    public void DecreaseStock_ZeroQuantity_ThrowsInvalidQuantityException()
    {
        var product = SampleProducts.Create();

        Assert.Throws<InvalidQuantityException>(() => product.DecreaseStock(0));
    }
}

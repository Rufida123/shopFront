using Shopfront.Domain.Entities;
using Shopfront.Domain.Exceptions;
using Shopfront.Domain.ValueObjects;

namespace Shopfront.UnitTests.Domain;

public class OrderTests
{
    private static readonly DateTimeOffset PlacedAt = new(2026, 10, 1, 9, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Place_NoLines_ThrowsEmptyOrderException()
    {
        Assert.Throws<EmptyOrderException>(() => Order.Place(Guid.NewGuid(), [], PlacedAt));
    }

    [Fact]
    public void Place_TwoProducts_CalculatesTotalFromLineTotals()
    {
        var lamp = SampleProducts.Create("Desk Lamp", 10m, 10);
        var notebook = SampleProducts.Create("Notebook", 4.50m, 10);

        var order = Order.Place(Guid.NewGuid(), [new OrderLine(lamp, 3), new OrderLine(notebook, 2)], PlacedAt);

        Assert.Equal(39m, order.Total.Amount);
        Assert.Equal(2, order.Items.Count);
    }

    [Fact]
    public void Place_ValidLines_DecreasesStockForEachProduct()
    {
        var lamp = SampleProducts.Create(stock: 10);

        Order.Place(Guid.NewGuid(), [new OrderLine(lamp, 4)], PlacedAt);

        Assert.Equal(6, lamp.StockQuantity);
    }

    [Fact]
    public void Place_PriceChangesAfterwards_KeepsOriginalPriceOnTheOrder()
    {
        var lamp = SampleProducts.Create("Desk Lamp", 25m, 10);
        var order = Order.Place(Guid.NewGuid(), [new OrderLine(lamp, 2)], PlacedAt);

        lamp.ChangePrice(Money.Of(40m));

        var item = Assert.Single(order.Items);
        Assert.Equal("Desk Lamp", item.ProductName);
        Assert.Equal(25m, item.UnitPrice.Amount);
        Assert.Equal(50m, order.Total.Amount);
    }

    [Fact]
    public void Place_QuantityAboveStock_ThrowsInsufficientStockException()
    {
        var lamp = SampleProducts.Create(stock: 2);

        Assert.Throws<InsufficientStockException>(
            () => Order.Place(Guid.NewGuid(), [new OrderLine(lamp, 3)], PlacedAt));
    }
}

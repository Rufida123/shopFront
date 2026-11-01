using Shopfront.Domain.Exceptions;
using Shopfront.Domain.ValueObjects;

namespace Shopfront.Domain.Entities;

public sealed class Product
{
    public const int NameMaxLength = 200;
    public const int DescriptionMaxLength = 2000;

    private Product()
    {
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public Money Price { get; private set; } = Money.Zero();

    public int StockQuantity { get; private set; }

    public static Product Create(string name, string description, Money price, int stockQuantity)
    {
        EnsurePriceIsPositive(price);

        if (stockQuantity < 0)
        {
            throw new InvalidStockQuantityException(stockQuantity);
        }

        return new Product
        {
            Id = Guid.CreateVersion7(),
            Name = name.Trim(),
            Description = description.Trim(),
            Price = price,
            StockQuantity = stockQuantity
        };
    }

    public void ChangePrice(Money newPrice)
    {
        EnsurePriceIsPositive(newPrice);
        Price = newPrice;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new InvalidQuantityException(quantity);
        }

        if (quantity > StockQuantity)
        {
            throw new InsufficientStockException(Name, StockQuantity);
        }

        StockQuantity -= quantity;
    }

    private static void EnsurePriceIsPositive(Money price)
    {
        if (price.Amount <= 0)
        {
            throw new InvalidPriceException(price.Amount);
        }
    }
}

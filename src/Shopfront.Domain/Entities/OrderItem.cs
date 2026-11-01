using Shopfront.Domain.ValueObjects;

namespace Shopfront.Domain.Entities;

public sealed class OrderItem
{
    private OrderItem()
    {
    }

    public Guid Id { get; private set; }

    public Guid ProductId { get; private set; }

    public string ProductName { get; private set; } = string.Empty;

    public Money UnitPrice { get; private set; } = Money.Zero();

    public int Quantity { get; private set; }

    public Money LineTotal => UnitPrice.Multiply(Quantity);

    public static OrderItem Create(Product product, int quantity) => new()
    {
        Id = Guid.CreateVersion7(),
        ProductId = product.Id,
        ProductName = product.Name,
        UnitPrice = Money.Of(product.Price.Amount, product.Price.Currency),
        Quantity = quantity
    };
}

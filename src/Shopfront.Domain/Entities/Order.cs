using Shopfront.Domain.Exceptions;
using Shopfront.Domain.ValueObjects;

namespace Shopfront.Domain.Entities;

public sealed class Order
{
    private readonly List<OrderItem> items = [];

    private Order()
    {
    }

    public Guid Id { get; private set; }

    public Guid CustomerId { get; private set; }

    public DateTimeOffset PlacedAt { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => items;

    public Money Total => items.Aggregate(Money.Zero(), (sum, item) => sum.Add(item.LineTotal));

    public static Order Place(Guid customerId, IReadOnlyCollection<OrderLine> lines, DateTimeOffset placedAt)
    {
        if (lines.Count == 0)
        {
            throw new EmptyOrderException();
        }

        var order = new Order
        {
            Id = Guid.CreateVersion7(),
            CustomerId = customerId,
            PlacedAt = placedAt
        };

        foreach (var line in lines)
        {
            line.Product.DecreaseStock(line.Quantity);
            order.items.Add(OrderItem.Create(line.Product, line.Quantity));
        }

        return order;
    }
}

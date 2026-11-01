using Shopfront.Domain.Entities;
using Shopfront.Domain.ValueObjects;

namespace Shopfront.UnitTests;

internal static class SampleProducts
{
    public static Product Create(string name = "Desk Lamp", decimal price = 25m, int stock = 10) =>
        Product.Create(name, "Sample description", Money.Of(price), stock);
}

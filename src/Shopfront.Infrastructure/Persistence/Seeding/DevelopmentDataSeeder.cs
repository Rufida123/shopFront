using Microsoft.EntityFrameworkCore;
using Shopfront.Domain.Entities;
using Shopfront.Domain.ValueObjects;

namespace Shopfront.Infrastructure.Persistence.Seeding;

public static class DevelopmentDataSeeder
{
    public static async Task SeedAsync(ShopfrontDbContext context, CancellationToken cancellationToken)
    {
        if (await context.Products.AnyAsync(cancellationToken))
        {
            return;
        }

        context.Products.AddRange(
            Product.Create("Desk Lamp", "Adjustable LED desk lamp", Money.Of(34.90m), 25),
            Product.Create("Floor Lamp", "Tall lamp with a linen shade", Money.Of(79.00m), 8),
            Product.Create("Standing Desk", "Height adjustable oak desk", Money.Of(449.00m), 5),
            Product.Create("Office Chair", "Mesh back chair with lumbar support", Money.Of(189.50m), 12),
            Product.Create("Notebook", "A5 dotted notebook, 160 pages", Money.Of(9.99m), 120),
            Product.Create("Fountain Pen", "Steel nib fountain pen", Money.Of(24.00m), 40));

        await context.SaveChangesAsync(cancellationToken);
    }
}

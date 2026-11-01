using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shopfront.Infrastructure.Persistence.Seeding;

namespace Shopfront.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(
        this IServiceProvider services,
        CancellationToken cancellationToken = default)
    {
        await using var scope = services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ShopfrontDbContext>();

        await context.Database.MigrateAsync(cancellationToken);
        await DevelopmentDataSeeder.SeedAsync(context, cancellationToken);
    }
}

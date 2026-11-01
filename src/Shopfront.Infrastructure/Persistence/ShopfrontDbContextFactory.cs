using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Shopfront.Infrastructure.Persistence;

public sealed class ShopfrontDbContextFactory : IDesignTimeDbContextFactory<ShopfrontDbContext>
{
    private const string LocalConnectionString =
        "Host=localhost;Port=5432;Database=shopfront;Username=postgres;Password=postgres";

    public ShopfrontDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ShopfrontDbContext>()
            .UseNpgsql(LocalConnectionString)
            .Options;

        return new ShopfrontDbContext(options);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shopfront.Application.Abstractions;
using Shopfront.Infrastructure.Persistence;
using Shopfront.Infrastructure.Persistence.Repositories;

namespace Shopfront.Infrastructure;

public static class DependencyInjection
{
    private const string ConnectionStringName = "Shopfront";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{ConnectionStringName}' is not configured");

        services.AddDbContext<ShopfrontDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ShopfrontDbContext>());
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}

using Microsoft.EntityFrameworkCore;
using Shopfront.Application.Abstractions;
using Shopfront.Domain.Entities;

namespace Shopfront.Infrastructure.Persistence;

public sealed class ShopfrontDbContext(DbContextOptions<ShopfrontDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Product> Products => Set<Product>();

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopfrontDbContext).Assembly);
}

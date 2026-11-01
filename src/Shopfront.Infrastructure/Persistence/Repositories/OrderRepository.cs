using Microsoft.EntityFrameworkCore;
using Shopfront.Application.Abstractions;
using Shopfront.Domain.Entities;

namespace Shopfront.Infrastructure.Persistence.Repositories;

public sealed class OrderRepository(ShopfrontDbContext context) : IOrderRepository
{
    public async Task AddAsync(Order order, CancellationToken cancellationToken) =>
        await context.Orders.AddAsync(order, cancellationToken);

    public Task<Order?> GetByIdReadOnlyAsync(Guid id, CancellationToken cancellationToken) =>
        context.Orders.AsNoTracking().FirstOrDefaultAsync(order => order.Id == id, cancellationToken);
}

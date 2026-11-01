using Shopfront.Domain.Entities;

namespace Shopfront.Application.Abstractions;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);

    Task<Order?> GetByIdReadOnlyAsync(Guid id, CancellationToken cancellationToken);
}

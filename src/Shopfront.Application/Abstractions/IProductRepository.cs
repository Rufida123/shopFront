using Shopfront.Application.Pagination;
using Shopfront.Domain.Entities;

namespace Shopfront.Application.Abstractions;

public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken);

    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Product?> GetByIdReadOnlyAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Product>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);

    Task<PagedResult<Product>> SearchAsync(
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
}

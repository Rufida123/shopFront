using Microsoft.EntityFrameworkCore;
using Shopfront.Application.Abstractions;
using Shopfront.Application.Pagination;
using Shopfront.Domain.Entities;

namespace Shopfront.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository(ShopfrontDbContext context) : IProductRepository
{
    public async Task AddAsync(Product product, CancellationToken cancellationToken) =>
        await context.Products.AddAsync(product, cancellationToken);

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

    public Task<Product?> GetByIdReadOnlyAsync(Guid id, CancellationToken cancellationToken) =>
        context.Products.AsNoTracking().FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Product>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken) =>
        await context.Products
            .Where(product => ids.Contains(product.Id))
            .ToListAsync(cancellationToken);

    public async Task<PagedResult<Product>> SearchAsync(
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = context.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(product => product.Name.ToLower().Contains(term));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(product => product.Name)
            .ThenBy(product => product.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Product>(items, page, pageSize, totalCount);
    }
}

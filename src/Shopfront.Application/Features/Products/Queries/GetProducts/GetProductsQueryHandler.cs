using MediatR;
using Shopfront.Application.Abstractions;
using Shopfront.Application.Dtos;
using Shopfront.Application.Mappings;
using Shopfront.Application.Pagination;

namespace Shopfront.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler(IProductRepository products)
    : IRequestHandler<GetProductsQuery, PagedResult<ProductDto>>
{
    public async Task<PagedResult<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var page = await products.SearchAsync(request.Search, request.Page, request.PageSize, cancellationToken);

        var items = page.Items.Select(product => product.ToDto()).ToList();
        return new PagedResult<ProductDto>(items, page.Page, page.PageSize, page.TotalCount);
    }
}

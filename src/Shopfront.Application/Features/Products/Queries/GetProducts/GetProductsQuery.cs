using MediatR;
using Shopfront.Application.Dtos;
using Shopfront.Application.Pagination;

namespace Shopfront.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsQuery(
    string? Search = null,
    int Page = GetProductsQuery.FirstPage,
    int PageSize = GetProductsQuery.DefaultPageSize) : IRequest<PagedResult<ProductDto>>
{
    public const int FirstPage = 1;
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 100;
}

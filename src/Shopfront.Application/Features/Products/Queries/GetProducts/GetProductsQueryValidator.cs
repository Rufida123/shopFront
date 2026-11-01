using FluentValidation;
using Shopfront.Domain.Entities;

namespace Shopfront.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    {
        RuleFor(query => query.Search).MaximumLength(Product.NameMaxLength);
        RuleFor(query => query.Page).GreaterThanOrEqualTo(GetProductsQuery.FirstPage);
        RuleFor(query => query.PageSize).InclusiveBetween(1, GetProductsQuery.MaxPageSize);
    }
}

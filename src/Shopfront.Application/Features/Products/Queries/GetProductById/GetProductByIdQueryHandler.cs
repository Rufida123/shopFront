using MediatR;
using Shopfront.Application.Abstractions;
using Shopfront.Application.Dtos;
using Shopfront.Application.Exceptions;
using Shopfront.Application.Mappings;

namespace Shopfront.Application.Features.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler(IProductRepository products)
    : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await products.GetByIdReadOnlyAsync(request.Id, cancellationToken)
            ?? throw new ProductNotFoundException(request.Id);

        return product.ToDto();
    }
}

using MediatR;
using Shopfront.Application.Abstractions;
using Shopfront.Application.Dtos;
using Shopfront.Application.Mappings;
using Shopfront.Domain.Entities;
using Shopfront.Domain.ValueObjects;

namespace Shopfront.Application.Features.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler(IProductRepository products, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            request.Name,
            request.Description,
            Money.Of(request.Price),
            request.StockQuantity);

        await products.AddAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return product.ToDto();
    }
}

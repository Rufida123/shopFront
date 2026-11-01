using MediatR;
using Shopfront.Application.Abstractions;
using Shopfront.Application.Dtos;
using Shopfront.Application.Exceptions;
using Shopfront.Application.Mappings;
using Shopfront.Domain.ValueObjects;

namespace Shopfront.Application.Features.Products.Commands.ChangeProductPrice;

public sealed class ChangeProductPriceCommandHandler(IProductRepository products, IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeProductPriceCommand, ProductDto>
{
    public async Task<ProductDto> Handle(ChangeProductPriceCommand request, CancellationToken cancellationToken)
    {
        var product = await products.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new ProductNotFoundException(request.Id);

        product.ChangePrice(Money.Of(request.Price));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return product.ToDto();
    }
}

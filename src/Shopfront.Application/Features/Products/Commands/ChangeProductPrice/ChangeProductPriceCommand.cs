using MediatR;
using Shopfront.Application.Dtos;

namespace Shopfront.Application.Features.Products.Commands.ChangeProductPrice;

public sealed record ChangeProductPriceCommand(Guid Id, decimal Price) : IRequest<ProductDto>;

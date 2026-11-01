using MediatR;
using Shopfront.Application.Dtos;

namespace Shopfront.Application.Features.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity) : IRequest<ProductDto>;

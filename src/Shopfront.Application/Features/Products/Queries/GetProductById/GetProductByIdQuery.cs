using MediatR;
using Shopfront.Application.Dtos;

namespace Shopfront.Application.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;

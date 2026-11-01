using Shopfront.Application.Dtos;
using Shopfront.Domain.Entities;

namespace Shopfront.Application.Mappings;

public static class ProductMapper
{
    public static ProductDto ToDto(this Product product) => new(
        product.Id,
        product.Name,
        product.Description,
        product.Price.ToDto(),
        product.StockQuantity);
}

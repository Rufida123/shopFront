using FluentValidation;
using Shopfront.Domain.Entities;

namespace Shopfront.Application.Features.Products.Commands.CreateProduct;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private const decimal MaxPrice = 1_000_000m;
    private const int MaxStockQuantity = 1_000_000;

    public CreateProductCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty().MaximumLength(Product.NameMaxLength);
        RuleFor(command => command.Description).MaximumLength(Product.DescriptionMaxLength);
        RuleFor(command => command.Price).LessThanOrEqualTo(MaxPrice);
        RuleFor(command => command.StockQuantity).LessThanOrEqualTo(MaxStockQuantity);
    }
}

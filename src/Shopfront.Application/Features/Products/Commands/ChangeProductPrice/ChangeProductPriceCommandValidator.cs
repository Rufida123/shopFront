using FluentValidation;

namespace Shopfront.Application.Features.Products.Commands.ChangeProductPrice;

public sealed class ChangeProductPriceCommandValidator : AbstractValidator<ChangeProductPriceCommand>
{
    private const decimal MaxPrice = 1_000_000m;

    public ChangeProductPriceCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty();
        RuleFor(command => command.Price).LessThanOrEqualTo(MaxPrice);
    }
}

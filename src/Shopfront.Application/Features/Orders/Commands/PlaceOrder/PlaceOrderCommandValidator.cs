using FluentValidation;

namespace Shopfront.Application.Features.Orders.Commands.PlaceOrder;

public sealed class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {
        RuleFor(command => command.CustomerId).NotEmpty();

        RuleFor(command => command.Lines)
            .Must(HaveEachProductOnce)
            .WithMessage("Each product can appear only once in an order");

        RuleForEach(command => command.Lines).ChildRules(line =>
        {
            line.RuleFor(item => item.ProductId).NotEmpty();
            line.RuleFor(item => item.Quantity).GreaterThan(0);
        });
    }

    private static bool HaveEachProductOnce(IReadOnlyList<OrderLineRequest> lines) =>
        lines.Select(line => line.ProductId).Distinct().Count() == lines.Count;
}

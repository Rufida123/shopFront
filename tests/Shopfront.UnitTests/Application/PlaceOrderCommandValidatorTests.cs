using FluentValidation.TestHelper;
using Shopfront.Application.Features.Orders.Commands.PlaceOrder;

namespace Shopfront.UnitTests.Application;

public class PlaceOrderCommandValidatorTests
{
    private readonly PlaceOrderCommandValidator validator = new();

    [Fact]
    public void Validate_ValidCommand_HasNoErrors()
    {
        var command = new PlaceOrderCommand(Guid.NewGuid(), [new OrderLineRequest(Guid.NewGuid(), 2)]);

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyCustomerId_HasErrorOnCustomerId()
    {
        var command = new PlaceOrderCommand(Guid.Empty, [new OrderLineRequest(Guid.NewGuid(), 2)]);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(item => item.CustomerId);
    }

    [Fact]
    public void Validate_SameProductTwice_HasErrorOnLines()
    {
        var productId = Guid.NewGuid();
        var command = new PlaceOrderCommand(
            Guid.NewGuid(),
            [new OrderLineRequest(productId, 1), new OrderLineRequest(productId, 2)]);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(item => item.Lines);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_QuantityNotAboveZero_HasErrorOnLineQuantity(int quantity)
    {
        var command = new PlaceOrderCommand(Guid.NewGuid(), [new OrderLineRequest(Guid.NewGuid(), quantity)]);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor("Lines[0].Quantity");
    }
}

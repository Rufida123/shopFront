using FluentValidation.TestHelper;
using Shopfront.Application.Features.Products.Commands.CreateProduct;
using Shopfront.Domain.Entities;

namespace Shopfront.UnitTests.Application;

public class CreateProductCommandValidatorTests
{
    private readonly CreateProductCommandValidator validator = new();

    [Fact]
    public void Validate_ValidCommand_HasNoErrors()
    {
        var command = new CreateProductCommand("Desk Lamp", "Warm light", 25m, 10);

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyName_HasErrorOnName()
    {
        var command = new CreateProductCommand("", "Warm light", 25m, 10);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(item => item.Name);
    }

    [Fact]
    public void Validate_NameAboveMaximumLength_HasErrorOnName()
    {
        var name = new string('a', Product.NameMaxLength + 1);
        var command = new CreateProductCommand(name, "Warm light", 25m, 10);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(item => item.Name);
    }
}

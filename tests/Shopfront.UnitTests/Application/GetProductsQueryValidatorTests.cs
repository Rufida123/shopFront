using FluentValidation.TestHelper;
using Shopfront.Application.Features.Products.Queries.GetProducts;

namespace Shopfront.UnitTests.Application;

public class GetProductsQueryValidatorTests
{
    private readonly GetProductsQueryValidator validator = new();

    [Fact]
    public void Validate_DefaultQuery_HasNoErrors()
    {
        var result = validator.TestValidate(new GetProductsQuery());

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_PageBelowOne_HasErrorOnPage()
    {
        var result = validator.TestValidate(new GetProductsQuery(Page: 0));

        result.ShouldHaveValidationErrorFor(query => query.Page);
    }

    [Fact]
    public void Validate_PageSizeAboveMaximum_HasErrorOnPageSize()
    {
        var result = validator.TestValidate(new GetProductsQuery(PageSize: GetProductsQuery.MaxPageSize + 1));

        result.ShouldHaveValidationErrorFor(query => query.PageSize);
    }
}

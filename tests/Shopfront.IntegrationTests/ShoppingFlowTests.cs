using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Shopfront.Application.Dtos;
using Shopfront.Application.Features.Orders.Commands.PlaceOrder;
using Shopfront.Application.Features.Products.Commands.CreateProduct;
using Shopfront.WebApi.Contracts;

namespace Shopfront.IntegrationTests;

public class ShoppingFlowTests(ShopfrontApiFactory factory) : IClassFixture<ShopfrontApiFactory>
{
    private readonly HttpClient client = factory.CreateClient();

    [Fact]
    public async Task PlaceOrder_ForExistingProduct_CreatesOrderThatCanBeFetched()
    {
        var product = await CreateProductAsync(price: 25m, stock: 10);
        var command = new PlaceOrderCommand(Guid.NewGuid(), [new OrderLineRequest(product.Id, 2)]);

        var placeResponse = await client.PostAsJsonAsync("/api/orders", command);
        var placedOrder = await ReadBodyAsync<OrderDto>(placeResponse);
        var fetchResponse = await client.GetAsync($"/api/orders/{placedOrder.Id}");
        var fetchedOrder = await ReadBodyAsync<OrderDto>(fetchResponse);
        var updatedProduct = await ReadBodyAsync<ProductDto>(await client.GetAsync($"/api/products/{product.Id}"));

        Assert.Equal(HttpStatusCode.Created, placeResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, fetchResponse.StatusCode);
        Assert.Equal(placedOrder.Id, fetchedOrder.Id);
        Assert.Equal(50m, fetchedOrder.Total.Amount);
        Assert.Equal("Desk Lamp", Assert.Single(fetchedOrder.Items).ProductName);
        Assert.Equal(8, updatedProduct.StockQuantity);
    }

    [Fact]
    public async Task ChangePrice_AfterOrderIsPlaced_DoesNotChangeThePlacedOrder()
    {
        var product = await CreateProductAsync(price: 25m, stock: 10);
        var command = new PlaceOrderCommand(Guid.NewGuid(), [new OrderLineRequest(product.Id, 2)]);
        var placedOrder = await ReadBodyAsync<OrderDto>(await client.PostAsJsonAsync("/api/orders", command));

        var priceResponse = await client.PutAsJsonAsync($"/api/products/{product.Id}/price", new ChangeProductPriceRequest(40m));
        var fetchedOrder = await ReadBodyAsync<OrderDto>(await client.GetAsync($"/api/orders/{placedOrder.Id}"));

        Assert.Equal(HttpStatusCode.OK, priceResponse.StatusCode);
        Assert.Equal(50m, fetchedOrder.Total.Amount);
        Assert.Equal(25m, Assert.Single(fetchedOrder.Items).UnitPrice.Amount);
    }

    [Fact]
    public async Task PlaceOrder_QuantityAboveStock_ReturnsUnprocessableEntity()
    {
        var product = await CreateProductAsync(price: 10m, stock: 2);
        var command = new PlaceOrderCommand(Guid.NewGuid(), [new OrderLineRequest(product.Id, 3)]);

        var response = await client.PostAsJsonAsync("/api/orders", command);

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task PlaceOrder_NoLines_ReturnsUnprocessableEntity()
    {
        var command = new PlaceOrderCommand(Guid.NewGuid(), []);

        var response = await client.PostAsJsonAsync("/api/orders", command);

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task PlaceOrder_UnknownProduct_ReturnsNotFound()
    {
        var command = new PlaceOrderCommand(Guid.NewGuid(), [new OrderLineRequest(Guid.NewGuid(), 1)]);

        var response = await client.PostAsJsonAsync("/api/orders", command);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetProducts_PageBelowOne_ReturnsBadRequestWithErrors()
    {
        var response = await client.GetAsync("/api/products?page=0");

        var problem = await ReadBodyAsync<HttpValidationProblemDetails>(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("Page", problem.Errors.Keys);
    }

    [Fact]
    public async Task GetOrder_UnknownId_ReturnsNotFound()
    {
        var response = await client.GetAsync($"/api/orders/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private async Task<ProductDto> CreateProductAsync(decimal price, int stock)
    {
        var command = new CreateProductCommand("Desk Lamp", "Adjustable LED lamp", price, stock);
        var response = await client.PostAsJsonAsync("/api/products", command);

        return await ReadBodyAsync<ProductDto>(response);
    }

    private static async Task<T> ReadBodyAsync<T>(HttpResponseMessage response)
    {
        var body = await response.Content.ReadFromJsonAsync<T>();
        Assert.NotNull(body);
        return body;
    }
}

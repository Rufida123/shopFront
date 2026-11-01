using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Shopfront.Application.Abstractions;
using Shopfront.Application.Exceptions;
using Shopfront.Application.Features.Orders.Commands.PlaceOrder;
using Shopfront.Domain.Entities;
using Shopfront.Domain.Exceptions;

namespace Shopfront.UnitTests.Application;

public class PlaceOrderCommandHandlerTests
{
    private readonly IProductRepository products = Substitute.For<IProductRepository>();
    private readonly IOrderRepository orders = Substitute.For<IOrderRepository>();
    private readonly IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly PlaceOrderCommandHandler handler;

    public PlaceOrderCommandHandlerTests()
    {
        handler = new PlaceOrderCommandHandler(
            products,
            orders,
            unitOfWork,
            TimeProvider.System,
            NullLogger<PlaceOrderCommandHandler>.Instance);
    }

    [Fact]
    public async Task Handle_ValidLines_AddsOrderReducesStockAndSavesOnce()
    {
        var customerId = Guid.NewGuid();
        var lamp = SampleProducts.Create("Desk Lamp", 25m, 10);
        products.GetByIdsAsync(Arg.Any<IReadOnlyCollection<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new List<Product> { lamp });
        var command = new PlaceOrderCommand(customerId, [new OrderLineRequest(lamp.Id, 3)]);

        var result = await handler.Handle(command, CancellationToken.None);

        await orders.Received(1).AddAsync(
            Arg.Is<Order>(order => order.CustomerId == customerId && order.Items.Count == 1),
            Arg.Any<CancellationToken>());
        await unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        Assert.Equal(7, lamp.StockQuantity);
        Assert.Equal(75m, result.Total.Amount);
    }

    [Fact]
    public async Task Handle_NoLines_ThrowsEmptyOrderExceptionAndSavesNothing()
    {
        products.GetByIdsAsync(Arg.Any<IReadOnlyCollection<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new List<Product>());
        var command = new PlaceOrderCommand(Guid.NewGuid(), []);

        await Assert.ThrowsAsync<EmptyOrderException>(() => handler.Handle(command, CancellationToken.None));

        await orders.DidNotReceive().AddAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>());
        await unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_UnknownProduct_ThrowsProductNotFoundExceptionAndSavesNothing()
    {
        products.GetByIdsAsync(Arg.Any<IReadOnlyCollection<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new List<Product>());
        var command = new PlaceOrderCommand(Guid.NewGuid(), [new OrderLineRequest(Guid.NewGuid(), 1)]);

        await Assert.ThrowsAsync<ProductNotFoundException>(() => handler.Handle(command, CancellationToken.None));

        await unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}

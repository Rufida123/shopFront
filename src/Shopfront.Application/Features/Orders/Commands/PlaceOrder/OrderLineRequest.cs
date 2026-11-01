namespace Shopfront.Application.Features.Orders.Commands.PlaceOrder;

public sealed record OrderLineRequest(Guid ProductId, int Quantity);

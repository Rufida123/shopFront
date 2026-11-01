namespace Shopfront.Application.Exceptions;

public sealed class OrderNotFoundException(Guid id) : NotFoundException($"Order {id} was not found");

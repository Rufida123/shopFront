namespace Shopfront.Application.Exceptions;

public sealed class ProductNotFoundException(Guid id) : NotFoundException($"Product {id} was not found");

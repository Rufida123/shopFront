namespace Shopfront.Domain.Entities;

public sealed record OrderLine(Product Product, int Quantity);

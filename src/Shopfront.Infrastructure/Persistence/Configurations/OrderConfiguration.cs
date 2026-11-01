using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopfront.Domain.Entities;
using Shopfront.Domain.ValueObjects;

namespace Shopfront.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    private const int AmountPrecision = 18;

    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(order => order.Id);
        builder.Property(order => order.Id).ValueGeneratedNever();
        builder.HasIndex(order => order.CustomerId);
        builder.Ignore(order => order.Total);

        builder.OwnsMany(order => order.Items, items =>
        {
            items.ToTable("OrderItems");
            items.WithOwner().HasForeignKey("OrderId");
            items.HasKey(item => item.Id);
            items.Property(item => item.Id).ValueGeneratedNever();
            items.Property(item => item.ProductName).IsRequired().HasMaxLength(Product.NameMaxLength);
            items.Ignore(item => item.LineTotal);

            items.OwnsOne(item => item.UnitPrice, price =>
            {
                price.Property(money => money.Amount)
                    .HasColumnName("UnitPrice")
                    .HasPrecision(AmountPrecision, Money.DecimalPlaces);
                price.Property(money => money.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(Money.CurrencyCodeLength);
            });
            items.Navigation(item => item.UnitPrice).IsRequired();
        });
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopfront.Domain.Entities;
using Shopfront.Domain.ValueObjects;

namespace Shopfront.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    private const int AmountPrecision = 18;

    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(product => product.Id);
        builder.Property(product => product.Id).ValueGeneratedNever();
        builder.Property(product => product.Name).IsRequired().HasMaxLength(Product.NameMaxLength);
        builder.Property(product => product.Description).IsRequired().HasMaxLength(Product.DescriptionMaxLength);

        builder.OwnsOne(product => product.Price, price =>
        {
            price.Property(money => money.Amount)
                .HasColumnName("Price")
                .HasPrecision(AmountPrecision, Money.DecimalPlaces);
            price.Property(money => money.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(Money.CurrencyCodeLength);
        });
        builder.Navigation(product => product.Price).IsRequired();

        builder.HasIndex(product => product.Name);
    }
}

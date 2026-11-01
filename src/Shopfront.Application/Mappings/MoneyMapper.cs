using Shopfront.Application.Dtos;
using Shopfront.Domain.ValueObjects;

namespace Shopfront.Application.Mappings;

public static class MoneyMapper
{
    public static MoneyDto ToDto(this Money money) => new(money.Amount, money.Currency);
}

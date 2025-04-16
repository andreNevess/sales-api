using Ambev.DeveloperEvaluation.Application.DTOs;
using Bogus;

namespace Ambev.DeveloperEvaluation.Application.Fake
{
    public static class FakeSaleGenerator
    {
        public static List<SaleDto> GenerateFakeSales(int count = 10)
        {
            var itemFaker = new Faker<SaleItemDto>()
                .RuleFor(i => i.ProductId, f => f.Random.Guid().ToString())
                .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
                .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
                .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(5, 200));

            var saleFaker = new Faker<SaleDto>()
                .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(8).ToUpper())
                .RuleFor(s => s.SaleDate, f => f.Date.Recent(30))
                .RuleFor(s => s.Customer, f => f.Name.FullName())
                .RuleFor(s => s.Branch, f => f.Company.CompanyName())
                .RuleFor(s => s.Items, f => itemFaker.Generate(f.Random.Int(1, 3)));

            return saleFaker.Generate(count);
        }
    }
}

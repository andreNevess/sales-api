using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.UseCases;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Mappings;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sale
{
    public class SaleServiceTests
    {
        private SaleService CreateService(out DefaultContext context)
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new DefaultContext(options);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            var mapper = mapperConfig.CreateMapper();
            var discountService = new DiscountCalculatorService();

            return new SaleService(mapper, discountService, context);
        }

        [Fact]
        public async Task CreateSale_Should_ApplyDiscountCorrectly()
        {
            // Arrange
            var service = CreateService(out var context);
            var dto = new SaleDto
            {
                SaleNumber = "TST001",
                SaleDate = DateTime.UtcNow,
                Customer = "Tester",
                Branch = "Test Branch",
                Items = new List<SaleItemDto>
                {
                    new()
                    {
                        ProductId = "X",
                        ProductName = "Produto X",
                        Quantity = 5,
                        UnitPrice = 10
                    }
                }
            };

            // Act
            var result = await service.CreateSaleAsync(dto);

            // Assert
            result.TotalAmount.Should().Be(45);
            result.Items.First().Discount.Should().Be(0.1m);
        }
    }
}

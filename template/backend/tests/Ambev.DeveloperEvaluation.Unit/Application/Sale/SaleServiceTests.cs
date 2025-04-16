using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Mappings;
using Ambev.DeveloperEvaluation.Application.UseCases;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sale
{
    public class SaleServiceTests
    {
        private SaleService CreateService(out DefaultContext context)
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
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

        private SaleDto CreateTestSale(string saleNumber = "TST", int quantity = 5, decimal price = 10)
        {
            return new SaleDto
            {
                SaleNumber = saleNumber,
                SaleDate = DateTime.UtcNow,
                Customer = "Cliente Teste",
                Branch = "Filial Teste",
                Items = new List<SaleItemDto>
                {
                    new()
                    {
                        ProductId = "P1",
                        ProductName = "Produto Teste",
                        Quantity = quantity,
                        UnitPrice = price
                    }
                }
            };
        }

        #region CreateSaleAsync

        [Fact]
        public async Task CreateSale_Should_ApplyDiscountCorrectly()
        {
            var service = CreateService(out _);
            var dto = CreateTestSale("TST001", 5, 10);

            var result = await service.CreateSaleAsync(dto);

            result.TotalAmount.Should().Be(45); // 5 * 10 = 50 - 10%
            result.Items.First().Discount.Should().Be(0.1m);
        }

        [Fact]
        public async Task CreateSale_Should_Throw_When_Quantity_Exceeds_20()
        {
            var service = CreateService(out _);
            var dto = CreateTestSale("EXC001", 21);

            Func<Task> act = async () => await service.CreateSaleAsync(dto);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Não é permitido vender mais de 20 itens do mesmo produto.");
        }

        #endregion

        #region CancelSaleAsync

        [Fact]
        public async Task CancelSale_Should_Set_IsCancelled_To_True()
        {
            var service = CreateService(out var context);
            var sale = await service.CreateSaleAsync(CreateTestSale("CANCEL001"));

            var result = await service.CancelSaleAsync(sale.Id);

            result.Should().BeTrue();

            var updatedSale = await context.Sales.FindAsync(sale.Id);
            updatedSale!.IsCancelled.Should().BeTrue();
        }

        #endregion

        #region GetSaleByIdAsync

        [Fact]
        public async Task GetSaleById_Should_Return_SaleDto_When_Exists()
        {
            var service = CreateService(out _);
            var sale = await service.CreateSaleAsync(CreateTestSale("BYID001"));

            var result = await service.GetSaleByIdAsync(sale.Id);

            result.Should().NotBeNull();
            result!.SaleNumber.Should().Be("BYID001");
        }

        [Fact]
        public async Task GetSaleById_Should_Return_Null_When_NotFound()
        {
            var service = CreateService(out _);

            var result = await service.GetSaleByIdAsync(Guid.NewGuid());

            result.Should().BeNull();
        }

        #endregion

        #region GetAllSalesAsync

        [Fact]
        public async Task GetAllSales_Should_Return_List_With_Created_Sales()
        {
            var service = CreateService(out _);

            await service.CreateSaleAsync(CreateTestSale("LST-1", 5));
            await service.CreateSaleAsync(CreateTestSale("LST-2", 10));

            var sales = await service.GetAllSalesAsync();

            sales.Should().NotBeNull();
            sales.Should().HaveCount(2);
        }

        #endregion
    }
}

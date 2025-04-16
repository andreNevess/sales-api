using Ambev.DeveloperEvaluation.Domain.Services;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sale
{
    public class DiscountCalculatorServiceTests
    {
        private readonly DiscountCalculatorService _service = new();

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Zero_Discount_When_Quantity_Is_Less_Than_4(int quantity)
        {
            var discount = _service.CalculateDiscount(quantity);
            discount.Should().Be(0.0m);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(9)]
        public void Should_Return_10_Percent_Discount_When_Quantity_Is_Between_4_And_9(int quantity)
        {
            var discount = _service.CalculateDiscount(quantity);
            discount.Should().Be(0.10m);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(15)]
        [InlineData(20)]
        public void Should_Return_20_Percent_Discount_When_Quantity_Is_Between_10_And_20(int quantity)
        {
            var discount = _service.CalculateDiscount(quantity);
            discount.Should().Be(0.20m);
        }

        [Theory]
        [InlineData(21)]
        [InlineData(50)]
        public void Should_Throw_Exception_When_Quantity_Exceeds_20(int quantity)
        {
            var action = () => _service.CalculateDiscount(quantity);
            action.Should().Throw<InvalidOperationException>()
                .WithMessage("Não é permitido vender mais de 20 itens do mesmo produto.");
        }
    }
}

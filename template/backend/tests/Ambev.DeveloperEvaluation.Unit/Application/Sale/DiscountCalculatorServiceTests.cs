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
        public void Deve_Retornar_Zero_Para_Quantidade_Abaixo_De_4(int quantity)
        {
            var desconto = _service.CalculateDiscount(quantity);
            desconto.Should().Be(0.0m);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(9)]
        public void Deve_Aplicar_10_Porcento_De_Desconto_Para_Quantidade_Entre_4_E_9(int quantity)
        {
            var desconto = _service.CalculateDiscount(quantity);
            desconto.Should().Be(0.10m);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(15)]
        [InlineData(20)]
        public void Deve_Aplicar_20_Porcento_De_Desconto_Para_Quantidade_Entre_10_E_20(int quantity)
        {
            var desconto = _service.CalculateDiscount(quantity);
            desconto.Should().Be(0.20m);
        }

        [Theory]
        [InlineData(21)]
        [InlineData(50)]
        public void Deve_Lancar_Excecao_Para_Quantidade_Acima_De_20(int quantity)
        {
            var acao = () => _service.CalculateDiscount(quantity);
            acao.Should().Throw<InvalidOperationException>()
                .WithMessage("Não é permitido vender mais de 20 itens do mesmo produto.");
        }
    }
}

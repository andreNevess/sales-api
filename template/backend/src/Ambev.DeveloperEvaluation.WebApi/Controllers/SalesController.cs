using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.Application.Fake;

namespace Ambev.DeveloperEvaluation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly SaleService _saleService;
        private readonly string SALE_NOT_FOUND = "Venda não encontrada.";



        public SalesController(SaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] SaleDto saleDto)
        {
            try
            {
                var sale = await _saleService.CreateSaleAsync(saleDto);
                return Created(string.Empty, sale);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSales()
        {
            var sales = await _saleService.GetAllSalesAsync();
            return Ok(sales);
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelSale(Guid id)
        {
            var success = await _saleService.CancelSaleAsync(id);

            if (!success)
                return NotFound(new { message = SALE_NOT_FOUND });

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSaleById(Guid id)
        {
            var sale = await _saleService.GetSaleByIdAsync(id);

            if (sale == null)
                return NotFound(new { message = SALE_NOT_FOUND });

            return Ok(sale);
        }



        [HttpPost("seed")]
        public async Task<IActionResult> SeedFakeSales()
        {
            var fakeSales = FakeSaleGenerator.GenerateFakeSales(10);

            foreach (var sale in fakeSales)
                await _saleService.CreateSaleAsync(sale);

            return Ok(new { message = $"{fakeSales.Count} vendas fake criadas com sucesso!" });
        }




}

}

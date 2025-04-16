using Ambev.DeveloperEvaluation.Application.Fake;
using Ambev.DeveloperEvaluation.Application.UseCases;

namespace Ambev.DeveloperEvaluation.WebApi
{
    public static class StartupSeeder
    {
        public static async Task SeedAsync(IApplicationBuilder app)
        {
            var shouldSeed = Environment.GetEnvironmentVariable("SEED_FAKE_DATA");

            if (shouldSeed?.ToLower() != "true") return;

            using var scope = app.ApplicationServices.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<SaleService>();

            var fakeSales = FakeSaleGenerator.GenerateFakeSales(10);

            foreach (var sale in fakeSales)
                await service.CreateSaleAsync(sale);

            Console.WriteLine($"[FAKE SEED] {fakeSales.Count} vendas fake geradas com sucesso");
        }
    }
}

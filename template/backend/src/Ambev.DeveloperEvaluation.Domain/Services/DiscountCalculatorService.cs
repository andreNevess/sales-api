using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public class DiscountCalculatorService
    {
        public decimal CalculateDiscount(int quantity)
        {
            if (quantity < 4)
                return 0m;

            if (quantity >= 4 && quantity < 10)
                return 0.10m;

            if (quantity <= 20)
                return 0.20m;

            throw new InvalidOperationException("Não é permitido vender mais de 20 unidades de um mesmo item.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    using System;

    namespace Ambev.DeveloperEvaluation.Domain.Entities
    {
        public class SaleItem
        {
            public Guid Id { get; set; }
            public string ProductId { get; set; } = string.Empty;
            public string ProductName { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Discount { get; set; }

            public decimal Total => (UnitPrice * Quantity) * (1 - Discount);
        }
    }

}

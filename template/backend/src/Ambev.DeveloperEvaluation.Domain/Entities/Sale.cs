using Ambev.DeveloperEvaluation.Domain.Entities.Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public bool IsCancelled { get; set; }

        public List<SaleItem> Items { get; set; } = new();

        public decimal TotalAmount => Items.Sum(item => item.Total);
    }
}

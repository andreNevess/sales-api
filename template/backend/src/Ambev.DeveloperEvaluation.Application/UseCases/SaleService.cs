using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.UseCases
{
    public class SaleService
    {
        private readonly IMapper _mapper;
        private readonly DiscountCalculatorService _discountCalculator;
        private readonly DefaultContext _context;

        public SaleService(IMapper mapper, DiscountCalculatorService discountCalculator, DefaultContext context)
        {
            _mapper = mapper;
            _discountCalculator = discountCalculator;
            _context = context;
        }

        public async Task<Sale> CreateSaleAsync(SaleDto saleDto)
        {
            var sale = _mapper.Map<Sale>(saleDto);
            sale.SaleDate = DateTime.SpecifyKind(sale.SaleDate, DateTimeKind.Utc);

            foreach (var item in sale.Items)
            {
                item.Discount = _discountCalculator.CalculateDiscount(item.Quantity);
            }

            sale.Id = Guid.NewGuid();
            foreach (var item in sale.Items)
            {
                item.Id = Guid.NewGuid();
            }

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            return sale;
        }

        public async Task<List<SaleDto>> GetAllSalesAsync()
        {
            var sales = await _context.Sales
                .Include(s => s.Items)
                .ToListAsync();

            return _mapper.Map<List<SaleDto>>(sales);
        }

        public async Task<bool> CancelSaleAsync(Guid saleId)
        {
            var sale = await _context.Sales.FirstOrDefaultAsync(s => s.Id == saleId);

            if (sale == null)
                return false;

            sale.IsCancelled = true;

            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();

            return true;
        }



    }
}

using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
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

        public SaleService(IMapper mapper, DiscountCalculatorService discountCalculator)
        {
            _mapper = mapper;
            _discountCalculator = discountCalculator;
        }

        public Sale CreateSale(SaleDto saleDto)
        {
            var sale = _mapper.Map<Sale>(saleDto);

            foreach (var item in sale.Items)
            {
                item.Discount = _discountCalculator.CalculateDiscount(item.Quantity);
            }

            return sale;
        }
    }
}

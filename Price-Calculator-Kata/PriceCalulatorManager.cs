using Price_Calculator_Kata.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Price_Calculator_Kata
{
    public class PriceCalulatorManager
    {
        private readonly Product _product;

        public PriceCalulatorManager(Product product)
        {
            _product = product;
        }

        public decimal GetPriceWithTax()
        {
            var preTaxPrice = _product.Price;            
            var priceWithtax = preTaxPrice + (preTaxPrice * 0.2M);
            return Math.Round(priceWithtax, 2);
        }
    }
}

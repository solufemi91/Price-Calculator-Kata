using Price_Calculator_Kata.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Price_Calculator_Kata
{
    public class PriceCalculatorStringBuilder
    {
        public string GetProductDetailsText(Product product)
        {
            return $"These are the product details: Sample product: Book with name = {product.Name}, UPC={product.UPC}, price=${product.Price}";
        }
        public string GetTaxText(decimal preTaxPrice, decimal postTaxPrice)
        {
            return $"Product price reported as ${preTaxPrice} before tax and ${postTaxPrice} after 20% tax";
        }
    }
}

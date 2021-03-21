using Price_Calculator_Kata.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Price_Calculator_Kata
{
    public class PriceCalculatorStringBuilder
    {
        public string ProductDetailsIntro(Product product)
        {
            return $"These are the product details: Sample product: Book with name = {product.Name}, UPC={product.UPC}, price=${product.Price}.{Environment.NewLine}Enter option: {Environment.NewLine}1. Get tax  {Environment.NewLine}2. Get discount";
        }
        public string GetTaxText(decimal preTaxPrice, decimal postTaxPrice, string taxPercentage)
        {
            return $"Product price reported as ${preTaxPrice} before tax and ${postTaxPrice} after {taxPercentage}% tax";
        }

        public string GetDiscountPriceText(DiscountPrice discountPrice)
        {
            return $"Discount amount = ${discountPrice.DiscountAmount},{Environment.NewLine}Price = ${discountPrice.PriceAfterDiscount}";
        }

        public string GetDiscountPriceEntryPrompt()
        {
            return $"Enter discount percentage";
        }


        public string GetTaxPercentageEntryPrompt()
        {
            return $"Enter tax percentage ";
        }
    }
}

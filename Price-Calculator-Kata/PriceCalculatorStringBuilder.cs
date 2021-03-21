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
        public string GetTaxText(decimal preTaxPrice, decimal postTaxPrice)
        {
            return $"Product price reported as ${preTaxPrice} before tax and ${postTaxPrice} after 20% tax";
        }

        public string GetDiscountPriceText(DiscountPrice discountPrice)
        {
            return $"Tax=20%, discount={discountPrice.DiscountPercentage}%, {Environment.NewLine}Tax amount = ${discountPrice.TaxAmount}, Dicount amount = ${discountPrice.DiscountAmount}, {Environment.NewLine}Price before = ${discountPrice.PriceBeforeDiscount}, price after = ${discountPrice.PriceAfterDiscount}";
        }

        public string GetDiscountPriceEntryPrompt()
        {
            return $"Enter discount percentage";
        }
    }
}

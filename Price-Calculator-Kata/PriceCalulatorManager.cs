using Price_Calculator_Kata.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Price_Calculator_Kata
{
    public class PriceCalulatorManager
    {
        private readonly ProductDetailsManager _productDetailsManager;
        private readonly PriceCalculatorStringBuilder _priceCalculatorStringBuilder;

        public PriceCalulatorManager()
        {
            _priceCalculatorStringBuilder = new PriceCalculatorStringBuilder();
            _productDetailsManager = new ProductDetailsManager();
        }

        public void Init()
        {
            var product = _productDetailsManager.GetProduct();
            var productDetailsText = _priceCalculatorStringBuilder.ProductDetailsIntro(product);
            Console.WriteLine(productDetailsText);
            var selectedOption = Console.ReadLine();
            PrintCalculationMessage(selectedOption, product);
        }

        private void PrintCalculationMessage(string option, Product product)
        {
            decimal price;
            string message;

            if (option == "1")
            {
                price = GetPriceWithTax(product);
                message = _priceCalculatorStringBuilder.GetTaxText(product.Price, price);
                Console.WriteLine(message);
            }

            else if (option == "2")
            {
                message = _priceCalculatorStringBuilder.GetDiscountPriceEntryPrompt();
                Console.WriteLine(message);
                var discountPercentage = Console.ReadLine();
                var priceWithDiscount = GetPriceWithDiscount(discountPercentage, product);
                message = _priceCalculatorStringBuilder.GetDiscountPriceText(priceWithDiscount);
                Console.WriteLine(message);
            }
        }

        private DiscountPrice GetPriceWithDiscount(string discountLevel, Product product)
        {
            var taxAmount = Math.Round(GetPriceWithTax(product) - product.Price, 2);
            var discountPercentage = Math.Round(decimal.Parse(discountLevel), 2);
            var discountAmount = Math.Round((discountPercentage / 100) * product.Price, 2);

            return new DiscountPrice
            {
                DiscountPercentage = discountPercentage,
                TaxAmount = taxAmount,
                DiscountAmount = discountAmount,
                PriceBeforeDiscount = product.Price,
                PriceAfterDiscount = product.Price + (taxAmount - discountAmount)
            };
        }
      
        private decimal GetPriceWithTax(Product product)
        {
            var preTaxPrice = product.Price;
            var priceWithtax = preTaxPrice + (preTaxPrice * 0.2M);
            return Math.Round(priceWithtax, 2);
        }

    }
}

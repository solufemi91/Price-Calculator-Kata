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

            if (option == "1")
            {
                var taxPercentage = GetTaxPercentage();
                var priceWithTax = GetPriceWithTax(product, taxPercentage);
                Console.WriteLine(_priceCalculatorStringBuilder.GetTaxText(product.Price, priceWithTax, taxPercentage));
            }

            else if (option == "2")
            {
                var applicableUPCForSpecialDiscount = GetApplicableUPCForSpecialDiscount();
                var taxPercentage = GetTaxPercentage();
                var discountPercentage = GetDiscountPercentage();
                var priceWithDiscount = GetPriceWithDiscount(discountPercentage, applicableUPCForSpecialDiscount, taxPercentage, product);
                Console.WriteLine(_priceCalculatorStringBuilder.GetDiscountPriceText(priceWithDiscount));
            }
        }

        private string GetApplicableUPCForSpecialDiscount()
        {
            Console.WriteLine(_priceCalculatorStringBuilder.GetApplicableUPCDiscountEntryPrompt());
            return Console.ReadLine();
        }

        private string GetTaxPercentage()
        {
            Console.WriteLine(_priceCalculatorStringBuilder.GetTaxPercentageEntryPrompt());
            return  Console.ReadLine();
        }

        private string GetDiscountPercentage()
        {
            Console.WriteLine(_priceCalculatorStringBuilder.GetDiscountPriceEntryPrompt());
            return Console.ReadLine();
        }

        private DiscountPrice GetPriceWithDiscount(string discount, string applicableUPCForSpecialDiscount, string taxPercentage, Product product)
        {
            var taxAmount = Math.Round(GetPriceWithTax(product, taxPercentage) - product.Price, 2);
            var discountPercentage = Math.Round(decimal.Parse(discount), 2);
            var discountAmount = Math.Round((discountPercentage / 100) * product.Price, 2);

            if(product.UPC == int.Parse(applicableUPCForSpecialDiscount))
            {
                discountAmount += product.Price * 0.07M;
            }

            return new DiscountPrice
            {
                DiscountPercentage = discountPercentage,
                TaxAmount = taxAmount,
                DiscountAmount = discountAmount,
                PriceBeforeDiscount = product.Price,
                PriceAfterDiscount = product.Price + (taxAmount - discountAmount)
            };
        }

        private decimal GetPriceWithTax(Product product, string taxPercentage)
        {
            var preTaxPrice = product.Price;
            var multipier = Math.Round(decimal.Parse(taxPercentage), 2) / 100;
            var priceWithtax = preTaxPrice + (preTaxPrice * multipier);
            return Math.Round(priceWithtax, 2);
        }

    }
}

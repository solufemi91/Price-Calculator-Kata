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
                var priceWithTax = GetPriceAfterTaxApplied(product.Price, taxPercentage);
                Console.WriteLine(_priceCalculatorStringBuilder.GetTaxText(product.Price, priceWithTax, taxPercentage));
            }

            else if (option == "2")
            {
                var applicableUPCForSpecialDiscount = GetApplicableUPCForSpecialDiscount();
                var taxPercentage = GetTaxPercentage();
                var discountPercentage = GetDiscountPercentage();
                var applyDiscountFirst = GetApplyDiscountFirstFlag();

                var discountPrice = ApplyDiscountsAndTax(applyDiscountFirst, product, applicableUPCForSpecialDiscount, taxPercentage, discountPercentage);

                Console.WriteLine(_priceCalculatorStringBuilder.GetDiscountPriceText(discountPrice.TotalDiscountDeduction, discountPrice.TotalPriceAfterDiscount));
            }
        }

        private DiscountPrice ApplyDiscountsAndTax(string applyDiscountFirst, Product product, string applicableUPCForSpecialDiscount, string taxPercentage, string discountPercentage)
        {
            var upcDiscountDeduction = GetUPCDiscountDeduction(product, applicableUPCForSpecialDiscount);
            decimal totalDiscountDeduction;
            decimal totalPriceAfterDiscount;
            decimal taxAddition;
            decimal discountDeduction;

            if (applyDiscountFirst == "Y")
            {               
                var priceAfterUPCDiscount = product.Price - upcDiscountDeduction;
                taxAddition = GetTaxAddition(priceAfterUPCDiscount, taxPercentage);
                discountDeduction = GetDiscountDeduction(discountPercentage, priceAfterUPCDiscount);
                totalDiscountDeduction = discountDeduction + upcDiscountDeduction;
                totalPriceAfterDiscount = priceAfterUPCDiscount + taxAddition - discountDeduction;
            }
            else
            {
                taxAddition = GetTaxAddition(product.Price, taxPercentage);
                discountDeduction = GetDiscountDeduction(discountPercentage, product.Price);               
                totalDiscountDeduction = discountDeduction + upcDiscountDeduction;
                totalPriceAfterDiscount = product.Price + taxAddition - discountDeduction - upcDiscountDeduction;
            }

            return new DiscountPrice { TotalDiscountDeduction = totalDiscountDeduction, TotalPriceAfterDiscount = totalPriceAfterDiscount };

        }

      
        private decimal GetPriceAfterDiscountApplied(string discount, string applicableUPCForSpecialDiscount, string taxPercentage, decimal price, string applyDiscountFirst)
        {
            var discountAmount = GetDiscountDeduction(discount, price);
            var priceAfterDiscountApplied = price - discountAmount;
            return RoundToTwoDecimalPlaces(priceAfterDiscountApplied);
        }

        
        private decimal GetPriceAfterTaxApplied(decimal price, string tax)
        {
            var taxAmount = GetTaxAddition(price, tax);
            var priceWithtax = price + taxAmount;
            return RoundToTwoDecimalPlaces(priceWithtax);
        }

        private decimal GetTaxAddition(decimal price, string tax)
        {
            var taxPercentage = RoundToTwoDecimalPlaces(decimal.Parse(tax));
            return RoundToTwoDecimalPlaces(price * GetMultiplier(taxPercentage));
        }

        private decimal GetDiscountDeduction(string discount, decimal price)
        {
            var discountPercentage = RoundToTwoDecimalPlaces(decimal.Parse(discount));
            return RoundToTwoDecimalPlaces(price * GetMultiplier(discountPercentage));
        }

        private decimal GetUPCDiscountDeduction(Product product, string applicableUPCForSpecialDiscount)
        {
            var result =  product.UPC == int.Parse(applicableUPCForSpecialDiscount) ? product.Price * 0.07M : 0;
            return RoundToTwoDecimalPlaces(result);
        }

        private decimal RoundToTwoDecimalPlaces(decimal number)
        {
            return Math.Round(number, 2);
        }

        private decimal GetMultiplier(decimal d)
        {
            return d / 100;
        }

        private string GetApplicableUPCForSpecialDiscount()
        {
            Console.WriteLine(_priceCalculatorStringBuilder.GetApplicableUPCDiscountEntryPrompt());
            return Console.ReadLine();
        }

        private string GetTaxPercentage()
        {
            Console.WriteLine(_priceCalculatorStringBuilder.GetTaxPercentageEntryPrompt());
            return Console.ReadLine();
        }

        private string GetDiscountPercentage()
        {
            Console.WriteLine(_priceCalculatorStringBuilder.GetDiscountPriceEntryPrompt());
            return Console.ReadLine();
        }

        private string GetApplyDiscountFirstFlag()
        {
            Console.WriteLine(_priceCalculatorStringBuilder.ApplyDiscountFirstPrompt());
            return Console.ReadLine();
        }

    }
}

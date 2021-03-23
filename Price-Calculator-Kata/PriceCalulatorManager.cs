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
            var selectedOption = GetAnswer(productDetailsText);
            PrintCalculationMessage(selectedOption, product);
        }

        private void PrintCalculationMessage(string option, Product product)
        {

            if (option == "1")
            {
                var taxPercentage = GetTaxPercentage();
                var taxAddition = GetTaxAddition(product.Price, taxPercentage);
                var priceWithTax = GetPriceAfterTaxApplied(product.Price, taxPercentage);
                Console.WriteLine(_priceCalculatorStringBuilder.GetTaxText(product.Price, priceWithTax, taxAddition, taxPercentage));
            }

            else if (option == "2")
            {
                var applicableUPCForSpecialDiscount = GetApplicableUPCForSpecialDiscount();
                var taxPercentage = GetTaxPercentage();
                var discountPercentage = GetDiscountPercentage();
                var applyDiscountFirst = GetApplyDiscountFirstFlag();
                var priceDetails = ApplyDiscountsAndTax(applyDiscountFirst, product, applicableUPCForSpecialDiscount, taxPercentage, discountPercentage);
                var applyTransportCost = GetAnswerForApplyTransportCost();
                priceDetails.TransportCost = GetAdditionalExpenses(applyTransportCost, product);
                var applyPackagingCost = GetAnswerForApplyPackagingCost();
                priceDetails.PackagingCost = GetAdditionalExpenses(applyPackagingCost, product);
                priceDetails.TotalPriceAfterDiscountAndTaxation += (priceDetails.PackagingCost + priceDetails.TransportCost);

                Console.WriteLine(_priceCalculatorStringBuilder.GetDiscountPriceText(priceDetails));
            }
        }

        private decimal GetAdditionalExpenses(string additionalExpenses, Product product)
        {
            if (additionalExpenses == "Y")
            {
                var answer = GetApplyPercentage();
                if (answer == "Y")
                {
                    return product.Price * GetMultiplier(1);
                }
                else
                {
                    return 2.2M;
                }
            }

            return 0M;
        }

        private PriceDetails ApplyDiscountsAndTax(string applyDiscountFirst, Product product, string applicableUPCForSpecialDiscount, string taxPercentage, string discountPercentage)
        {
            var upcDiscountDeduction = GetUPCDiscountDeduction(product, applicableUPCForSpecialDiscount);
            decimal totalDiscountDeduction;
            decimal totalPriceAfterDiscountAndTaxation;
            decimal taxAddition;
            decimal discountDeduction;

            if (applyDiscountFirst == "Y")
            {               
                var priceAfterUPCDiscount = product.Price - upcDiscountDeduction;
                taxAddition = GetTaxAddition(priceAfterUPCDiscount, taxPercentage);
                discountDeduction = GetDiscountDeduction(discountPercentage, priceAfterUPCDiscount);
                totalDiscountDeduction = discountDeduction + upcDiscountDeduction;
                totalPriceAfterDiscountAndTaxation = priceAfterUPCDiscount + taxAddition - discountDeduction;
            }
            else
            {
                taxAddition = GetTaxAddition(product.Price, taxPercentage);
                discountDeduction = GetDiscountDeduction(discountPercentage, product.Price);               
                totalDiscountDeduction = discountDeduction + upcDiscountDeduction;
                totalPriceAfterDiscountAndTaxation = product.Price + taxAddition - discountDeduction - upcDiscountDeduction;
            }

            return new PriceDetails {
                TotalDiscountDeduction = totalDiscountDeduction,
                TotalPriceAfterDiscountAndTaxation = totalPriceAfterDiscountAndTaxation,
                TaxAddition = taxAddition,
                InitialPrice = product.Price
            };

        }
     
        private decimal GetPriceAfterTaxApplied(decimal price, string tax)
        {
            var taxAmount = GetTaxAddition(price, tax);
            var priceWithtax = price + taxAmount;
            return priceWithtax;
        }

        private decimal GetTaxAddition(decimal price, string tax)
        {
            var taxPercentage = decimal.Parse(tax);
            return price * GetMultiplier(taxPercentage);
        }

        private decimal GetDiscountDeduction(string discount, decimal price)
        {
            var discountPercentage = decimal.Parse(discount);
            return price * GetMultiplier(discountPercentage);
        }

        private decimal GetUPCDiscountDeduction(Product product, string applicableUPCForSpecialDiscount)
        {
            var result =  product.UPC == int.Parse(applicableUPCForSpecialDiscount) ? product.Price * 0.07M : 0;
            return result;
        }

     

        private decimal GetMultiplier(decimal d)
        {
            return d / 100;
        }

        private string GetAnswerForApplyPackagingCost()
        {
            return GetAnswer(_priceCalculatorStringBuilder.GetAnswerForApplyPackagingCostPrompt());
        }

        private string GetAnswerForApplyTransportCost()
        {
            return GetAnswer(_priceCalculatorStringBuilder.GetAnswerForApplyTransportCostPrompt());
        }
        private string GetApplicableUPCForSpecialDiscount()
        {
            return GetAnswer(_priceCalculatorStringBuilder.GetApplicableUPCDiscountEntryPrompt());
        }

        private string GetTaxPercentage()
        {
            return GetAnswer(_priceCalculatorStringBuilder.GetTaxPercentageEntryPrompt());
        }

        private string GetDiscountPercentage()
        {
            return GetAnswer(_priceCalculatorStringBuilder.GetDiscountPriceEntryPrompt());
        }

        private string GetApplyDiscountFirstFlag()
        {
            return GetAnswer(_priceCalculatorStringBuilder.ApplyDiscountFirstPrompt());
        }

        private string GetApplyPercentage()
        {
            return GetAnswer("apply percentage? Y/N");
        }

        private string GetAnswer(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }

    }
}

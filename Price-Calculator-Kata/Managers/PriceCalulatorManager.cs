using Microsoft.Extensions.Configuration;
using Price_Calculator_Kata.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Price_Calculator_Kata
{
    public class PriceCalulatorManager : IPriceCalulatorManager
    {
        private readonly IProductDetailsManager _productDetailsManager;
        private readonly IPriceCalculatorStringBuilder _priceCalculatorStringBuilder;
        private readonly IConfigurationWrapper _config;
        private readonly IConsoleWrapper _consoleWrapper;
        private readonly Product _product;

        public PriceCalulatorManager(IProductDetailsManager productDetailsManager, IPriceCalculatorStringBuilder priceCalculatorStringBuilder, IConfigurationWrapper config, IConsoleWrapper consoleWrapper)
        {
            _priceCalculatorStringBuilder = priceCalculatorStringBuilder;
            _productDetailsManager = productDetailsManager;
            _consoleWrapper = consoleWrapper;
            _product = _productDetailsManager.GetProduct();
            _config = config;
        }

        public void Init()
        {
            var taxOrDiscount = _priceCalculatorStringBuilder.TaxOrDisount(_product);          
            var selectedOption = _consoleWrapper.GetAnswer(taxOrDiscount);
            PrintCalculationMessage(selectedOption);
        }

        private void PrintCalculationMessage(string option)
        {

            if (option == "1")
            {
                var taxPercentage = GetTaxPercentage();
                var taxAddition = GetTaxAddition(_product.Price, taxPercentage);
                var priceWithTax = GetPriceAfterTaxApplied(_product.Price, taxPercentage);
                Console.WriteLine(_priceCalculatorStringBuilder.GetTaxText(_product.Price, priceWithTax, taxAddition, taxPercentage));
            }

            else if (option == "2")
            {
                var applicableUPCForSpecialDiscount = GetApplicableUPCForSpecialDiscount();
                var taxPercentage = GetTaxPercentage();
                var discountPercentage = GetDiscountPercentage();
                var applyDiscountFirst = GetApplyDiscountFirstFlag();
                var methodOfCombinedDiscount = GetMethodOfCombinedDiscount();
                var priceDetails = ApplyDiscountsAndTax(applyDiscountFirst, _product, applicableUPCForSpecialDiscount, taxPercentage, discountPercentage, methodOfCombinedDiscount);
                var applyTransportCost = GetAnswerForApplyTransportCost();
                priceDetails.TransportCost = GetAdditionalExpenses(applyTransportCost, _product);
                var applyPackagingCost = GetMethodForApplyPackagingCost();
                priceDetails.PackagingCost = GetAdditionalExpenses(applyPackagingCost, _product);
                priceDetails.TotalPriceAfterDiscountAndTaxation += (priceDetails.PackagingCost + priceDetails.TransportCost);

                Console.WriteLine(_priceCalculatorStringBuilder.GetDiscountPriceText(priceDetails));
            }
        }

        public decimal GetAdditionalExpenses(string additionalExpenses, Product product)
        {
            if (additionalExpenses == "Y")
            {
                var answer = GetApplyPercentage();
                if (answer == "Y")
                {
                    return product.Price * GetMultiplier(_config.AdditionalExpensesPercentage);
                }
                else
                {
                    return _config.AdditionalExpensesFixedAmount;
                }
            }
            else if (additionalExpenses == "N")
            {
                return 0M;
            }          
            
            return 0M;
        }

        private PriceDetails ApplyDiscountsAndTax(string applyDiscountFirst, Product product, string applicableUPCForSpecialDiscount, string taxPercentage,
            string discountPercentage, string methodOfCombinedDiscount)
        {
            var upcDiscountDeduction = GetUPCDiscountDeduction(product, applicableUPCForSpecialDiscount, methodOfCombinedDiscount, discountPercentage);
            decimal totalDiscountDeduction;
            decimal totalPriceAfterDiscountAndTaxation;
            decimal taxAddition;
            decimal universalDiscountDeduction;

            if (applyDiscountFirst == "Y")
            {               
                var priceAfterUPCDiscount = product.Price - upcDiscountDeduction;
                taxAddition = GetTaxAddition(priceAfterUPCDiscount, taxPercentage);
                universalDiscountDeduction = GetUniversalDiscountDeduction(discountPercentage, priceAfterUPCDiscount);
                totalDiscountDeduction = GetTotalDiscountDeduction(universalDiscountDeduction, upcDiscountDeduction, product.Price);
                totalPriceAfterDiscountAndTaxation = priceAfterUPCDiscount + taxAddition - totalDiscountDeduction;
            }
            else
            {
                taxAddition = GetTaxAddition(product.Price, taxPercentage);
                universalDiscountDeduction = GetUniversalDiscountDeduction(discountPercentage, product.Price);
                totalDiscountDeduction = GetTotalDiscountDeduction(universalDiscountDeduction, upcDiscountDeduction, product.Price);
                totalPriceAfterDiscountAndTaxation = product.Price + taxAddition - totalDiscountDeduction;
            }

            return new PriceDetails {
                TotalDiscountDeduction = totalDiscountDeduction,
                TotalPriceAfterDiscountAndTaxation = totalPriceAfterDiscountAndTaxation,
                TaxAddition = taxAddition,
                InitialPrice = product.Price
            };
        }


        private decimal GetTotalDiscountDeduction(decimal universalDiscountDeduction, decimal upcDiscountDeduction, decimal cost)
        {
            var methodOfCalculation = GetMethodOfCalculationForCap();
            decimal cap = 0;
            var totalDiscountDeduction = universalDiscountDeduction + upcDiscountDeduction;

            if (methodOfCalculation == "P")
            {
                cap = cost * GetMultiplier(_config.DiscountCapPercentage);
            }

            else if(methodOfCalculation == "F")
            {
                cap = _config.DiscountCapFixedAmount;             
            }

            return cap < totalDiscountDeduction ? cap : totalDiscountDeduction;
        }

        private decimal GetPriceAfterDiscountApplied(string discount, decimal price)
        {
            var discountAmount = GetUniversalDiscountDeduction(discount, price);
            var priceAfterDiscountApplied = price - discountAmount;
            return priceAfterDiscountApplied;
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

        private decimal GetUniversalDiscountDeduction(string discount, decimal price)
        {
            var discountPercentage = decimal.Parse(discount);
            return price * GetMultiplier(discountPercentage);
        }

        private decimal GetUPCDiscountDeduction(Product product, string applicableUPCForSpecialDiscount, string methodOfCombinedDiscount, string discountPercentage)
        {
            var applicableUpc = int.Parse(applicableUPCForSpecialDiscount);
            var upcDiscountMultiplier = GetMultiplier(_config.UpcDiscount);
            var initialPrice = product.Price;

            if (methodOfCombinedDiscount == "M" && product.UPC == applicableUpc)
            {
                return GetPriceAfterDiscountApplied(discountPercentage, initialPrice) * upcDiscountMultiplier;
            }

            return product.UPC == applicableUpc ? initialPrice * upcDiscountMultiplier : 0;

        }

        private decimal GetMultiplier(decimal d)
        {
            return d / 100;
        }

        private string GetMethodOfCombinedDiscount()
        {
            return _consoleWrapper.GetAnswer(_priceCalculatorStringBuilder.MethodOfCombinedDiscountPrompt());
        }

        private string GetMethodForApplyPackagingCost()
        {
            return _consoleWrapper.GetAnswer(_priceCalculatorStringBuilder.GetAnswerForApplyPackagingCostPrompt());
        }

        private string GetAnswerForApplyTransportCost()
        {
            return _consoleWrapper.GetAnswer(_priceCalculatorStringBuilder.GetAnswerForApplyTransportCostPrompt());
        }
        private string GetApplicableUPCForSpecialDiscount()
        {
            return _consoleWrapper.GetAnswer(_priceCalculatorStringBuilder.GetApplicableUPCDiscountEntryPrompt());
        }

        private string GetTaxPercentage()
        {
            return _consoleWrapper.GetAnswer(_priceCalculatorStringBuilder.GetTaxPercentageEntryPrompt());
        }

        private string GetDiscountPercentage()
        {
            return _consoleWrapper.GetAnswer(_priceCalculatorStringBuilder.GetDiscountPriceEntryPrompt());
        }

        private string GetApplyDiscountFirstFlag()
        {
            return _consoleWrapper.GetAnswer(_priceCalculatorStringBuilder.ApplyDiscountFirstPrompt());
        }

        private string GetApplyPercentage()
        {
            return _consoleWrapper.GetAnswer(_priceCalculatorStringBuilder.ApplyPercentagePrompt());
        }

        private string GetMethodOfCalculationForCap()
        {
            return _consoleWrapper.GetAnswer(_priceCalculatorStringBuilder.MethodOfCalculationForCapPrompt());
        }


    }
}

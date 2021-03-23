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
        public string GetTaxText(decimal preTaxPrice, decimal postTaxPrice, decimal taxAddition, string taxPercentage)
        {
            var cost = BuildReportCategory(preTaxPrice, "Cost");
            var tax = BuildReportCategory(taxAddition, "Tax");
            var total = BuildReportCategory(postTaxPrice, "Total");

            return $"Product price reported as ${RoundToTwoDecimalPlaces(preTaxPrice)} before tax and ${RoundToTwoDecimalPlaces(postTaxPrice)} after {taxPercentage}% tax" +
             $"{cost}" +
             $"{tax}" +
             $"{total}";
        }

        public string GetDiscountPriceText(PriceDetails priceDetails)
        {
            var cost = BuildReportCategory(priceDetails.InitialPrice, "Cost");
            var tax = BuildReportCategory(priceDetails.TaxAddition, "Tax");
            var discounts = BuildReportCategory(priceDetails.TotalDiscountDeduction, "Discounts");
            var packaging = BuildReportCategory(priceDetails.PackagingCost, "Packaging");
            var transport = BuildReportCategory(priceDetails.TransportCost, "Transport"); 
            var total = BuildReportCategory(priceDetails.TotalPriceAfterDiscountAndTaxation, "Total"); 

            return $"{cost}" +
                $"{tax}" +
                $"{discounts}" +
                $"{packaging}" +
                $"{transport}" +
                $"{total}";
        }

        private string BuildReportCategory(decimal cost, string category)
        {
            return RoundToTwoDecimalPlaces(cost) != 0 ? $"{Environment.NewLine}{category} = ${RoundToTwoDecimalPlaces(cost)}" : string.Empty;
        }

        public string GetDiscountPriceEntryPrompt()
        {
            return $"Enter discount percentage";
        }


        public string GetTaxPercentageEntryPrompt()
        {
            return $"Enter tax percentage ";
        }

        public string GetApplicableUPCDiscountEntryPrompt()
        {
            return $"Enter applicable UPC for special discount";
        }

        public string GetAnswerForApplyPackagingCostPrompt()
        {
            return $"Apply Packaging cost? Y/N";
        }

        public string GetAnswerForApplyTransportCostPrompt()
        {
            return $"Apply Transport cost? Y/N";
        }

        public string ApplyDiscountFirstPrompt()
        {
            return $"Apply UPC discount first? Y for Yes, N for No";
        }

        private decimal RoundToTwoDecimalPlaces(decimal number)
        {
            return Math.Round(number, 2);
        }
    }
}

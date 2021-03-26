using Price_Calculator_Kata.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Price_Calculator_Kata
{
    public interface IPriceCalculatorStringBuilder
    {
        string TaxOrDisount(Product product);

        string GetTaxText(decimal preTaxPrice, decimal postTaxPrice, decimal taxAddition, string taxPercentage);

        string GetDiscountPriceText(PriceDetails priceDetails);

        string GetDiscountPriceEntryPrompt();

        string GetTaxPercentageEntryPrompt();

        string GetApplicableUPCDiscountEntryPrompt();

        string GetAnswerForApplyPackagingCostPrompt();

        string GetAnswerForApplyTransportCostPrompt();

        string ApplyDiscountFirstPrompt();

        string MethodOfCombinedDiscountPrompt();

        string ApplyPercentagePrompt();

        string MethodOfCalculationForCapPrompt();

    }
}

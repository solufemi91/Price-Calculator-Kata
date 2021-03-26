using System;
using System.Collections.Generic;
using System.Text;

namespace Price_Calculator_Kata
{
    public interface IConfigurationWrapper
    {
        int AdditionalExpensesPercentage { get; }

        decimal AdditionalExpensesFixedAmount { get; }

        decimal DiscountCapPercentage { get; }
        decimal DiscountCapFixedAmount { get; }

        decimal UpcDiscount { get; }
        string Currency { get; }
    }
}

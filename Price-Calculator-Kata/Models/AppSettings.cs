using System;
using System.Collections.Generic;
using System.Text;

namespace Price_Calculator_Kata.Models
{
    public class AppSettings
    {
        public string AdditionalExpensesPercentage { get; set; }
        public string AdditionalExpensesFixedAmount { get; set; }
        public string DiscountCapPercentage {get;set; }
        public string DiscountCapFixedAmount { get; set; }
        public string UpcDiscountPercentage { get; set; }
        public string Currency { get; set; }
    }
}

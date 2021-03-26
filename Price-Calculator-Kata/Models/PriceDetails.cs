using System;
using System.Collections.Generic;
using System.Text;

namespace Price_Calculator_Kata.Models
{
    public class PriceDetails
    {
        public decimal TotalDiscountDeduction { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal TaxAddition { get; set; }
        public decimal TotalPriceAfterDiscountAndTaxation { get; set; }
        public decimal TransportCost { get; set; }
        public decimal PackagingCost { get; set; }
    }
}

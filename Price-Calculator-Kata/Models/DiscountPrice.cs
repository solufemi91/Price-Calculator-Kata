using System;
using System.Collections.Generic;
using System.Text;

namespace Price_Calculator_Kata.Models
{
    public class DiscountPrice
    {
        public decimal DiscountPercentage { get; set; }
        public decimal TaxAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal PriceBeforeDiscount { get; set; }

        public decimal PriceAfterDiscount { get; set; }
    }
}

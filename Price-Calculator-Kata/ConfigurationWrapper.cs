using Microsoft.Extensions.Configuration;
using Price_Calculator_Kata.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Price_Calculator_Kata
{
    public class ConfigurationWrapper : IConfigurationWrapper
    {
        private readonly AppSettings _config;

        public ConfigurationWrapper(IConfiguration config)
        {
            _config = config.GetSection("AppSettings").Get<AppSettings>();
        }

        public int AdditionalExpensesPercentage => int.Parse(_config.AdditionalExpensesPercentage);
        public decimal AdditionalExpensesFixedAmount => decimal.Parse(_config.AdditionalExpensesFixedAmount);
        public decimal DiscountCapPercentage => decimal.Parse(_config.DiscountCapPercentage);
        public decimal DiscountCapFixedAmount => decimal.Parse(_config.DiscountCapFixedAmount);
        public decimal UpcDiscount => decimal.Parse(_config.UpcDiscountPercentage);
        public string Currency => _config.Currency;
    }
}

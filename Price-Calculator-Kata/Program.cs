using Price_Calculator_Kata.Models;
using System;

namespace Price_Calculator_Kata
{
    class Program
    {
        static void Main(string[] args)
        {
            var priceCalculatorStringBuilder = new PriceCalculatorStringBuilder();
            var productDetailsManager = new ProductDetailsManager();
            var product = productDetailsManager.GetProduct();
            Console.WriteLine(priceCalculatorStringBuilder.GetProductDetailsText(product));
            Console.WriteLine("Press any key to get the tax details");
            Console.ReadLine(); 
            var priceCalculatorManager = new PriceCalulatorManager(product);
            var priceWithTax = priceCalculatorManager.GetPriceWithTax();
            var taxText = priceCalculatorStringBuilder.GetTaxText(product.Price, priceWithTax);
            Console.WriteLine(taxText);
        }
    }
}

using Price_Calculator_Kata.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Price_Calculator_Kata
{
    public class ProductDetailsManager : IProductDetailsManager
    {
        public Product GetProduct()
        {
            return new Product { Name = "The Little Prince", Price = 20.25M, UPC = 12345 };
        }
    }
}

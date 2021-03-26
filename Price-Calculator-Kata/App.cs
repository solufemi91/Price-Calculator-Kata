using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Price_Calculator_Kata
{
    class App
    {
        private readonly IPriceCalulatorManager _priceCalulatorManager;

        public App(IPriceCalulatorManager priceCalulatorManager)
        {
            _priceCalulatorManager = priceCalulatorManager;
        }
        public void Run()
        {
            _priceCalulatorManager.Init();
        }
    }
}

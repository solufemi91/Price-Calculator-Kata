using System;
using System.Collections.Generic;
using System.Text;

namespace Price_Calculator_Kata
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public string GetAnswer(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }
    }
}

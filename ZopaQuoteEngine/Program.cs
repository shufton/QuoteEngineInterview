using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZopaQuoteEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            if (2 != args.Length)
            {
                PrintUsage();
                return;
            }

        }

        static void PrintUsage()
        {
            Console.WriteLine("Usage: Quote.exe [market_file] [loan_amount]\r\n"+
                              "       market_file : The current list of available lenders adn their rate/available amount\r\n" +
                              "       loan_amount : The loan amount that the application is to determine a quote offer for.");
        }
    }
}

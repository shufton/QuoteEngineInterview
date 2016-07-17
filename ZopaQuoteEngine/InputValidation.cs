using System;

namespace ZopaQuoteEngine
{
    internal class InputValidation
    {
        private const int LowerLoanAmount = 1000;
        private const int UpperLoanAmount = 15000;

        public int LoanAmount { get; private set; }
        public string FileName { get; private set; }
        internal bool ValidateInputs(string[] args)
        {
            if (2 != args.Length)
            {
                PrintUsage();
                return false;
            }

            var loanAmount = 0;
            var convertOk = Int32.TryParse(args[1], out loanAmount);
            if (!convertOk
                || loanAmount < LowerLoanAmount
                || loanAmount > UpperLoanAmount)
            {
                PrintUsage();
                return false;
            }

            var remainder = loanAmount % 100;
            if (0 != remainder)
            {
                PrintUsage();
                return false;
            }

            LoanAmount = loanAmount;
            FileName = args[0];

            return true;
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: Quote.exe [market_file] [loan_amount]\r\n" +
                              "       market_file : The current list of available lenders adn their rate/available amount\r\n" +
                              "       loan_amount : The loan amount that the application is to determine a quote offer for." +
                              "                      This must be between £1,000 and £15,000 rounded to the nearest £100.");
        }
    }
}

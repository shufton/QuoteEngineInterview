using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZopaQuoteEngine
{
    internal class QuoteResult
    {
        internal QuoteResult(QuoteError noQuoteReason)
        {
            IsValid = false;
            NoQuoteReason = noQuoteReason;
        }

        internal QuoteResult(double amount, double rate, double monthlyRepayment, double totalRepayment)
        {
            IsValid = true;
            Amount = amount;
            Rate = rate;
            MontlyRepayment = monthlyRepayment;
            TotalRepayment = totalRepayment;
        }

        public bool IsValid { get; private set; }
        public double Amount { get; private set; }
        public double Rate { get; private set; }
        public double MontlyRepayment { get; private set; }
        public double TotalRepayment { get; private set; }
        public QuoteError NoQuoteReason { get; private set; }
    }
}

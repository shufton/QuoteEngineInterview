using System;
using System.Collections.Generic;
using System.Linq;

namespace ZopaQuoteEngine
{
    class QuoteCalculationEngine
    {
        private int TermInMonths { get; set; }

        internal QuoteCalculationEngine(int termInMonths)
        {
            TermInMonths = termInMonths;
        }
        internal QuoteResult RequestQuote(List<Quote> quotes, double loanAmount)
        {
            if (null == quotes || 0 == quotes.Count)
                return new QuoteResult(QuoteError.NoQuotesAvailable);

            if (!IsCapitalAvailable(quotes, loanAmount))
                return new QuoteResult(QuoteError.InsufficientMarketAvailable);

            var sortedQuotes = ArrangeQuotesInOrderOfRate(quotes);
            var filteredQuotes = FilterLendersToSatisfyLoanRequest(sortedQuotes, loanAmount);
            var totalInterest = CalculateInterest(filteredQuotes, loanAmount);

            var totalRepayable = loanAmount + totalInterest;
            var monthlyRepayment = totalRepayable / 36;
            var apr = CalculateApr(loanAmount, totalRepayable);

            return new QuoteResult(loanAmount, apr, monthlyRepayment, totalRepayable);
        }

        internal List<Quote> ArrangeQuotesInOrderOfRate(List<Quote> inputQuotes)
        {
            inputQuotes.Sort((x, y) =>
            {
                if (Math.Abs(x.Rate - y.Rate) < 0.001)
                    // Reverse the order of x,y here to give priority to lenders who put more capital into system
                    return y.Amount.CompareTo(x.Amount);
                return x.Rate.CompareTo(y.Rate);
            });
            return inputQuotes;
        }

        internal bool IsCapitalAvailable(List<Quote> inputQuotes, double loanAmount)
        {
            return loanAmount <= inputQuotes.Sum(quote => quote.Amount);
        }

        internal List<Quote> FilterLendersToSatisfyLoanRequest(List<Quote> sortedQuotes, double loanAmount)
        {
            var lenderList = new List<Quote>();
            var runningTotal = 0.0;

            foreach (var quote in sortedQuotes)
            {
                lenderList.Add(quote);
                runningTotal += quote.Amount;
                if (runningTotal >= loanAmount)
                    break;
            }

            return lenderList;
        }

        internal double CalculateInterest(Quote quote, double loanAmount)
        {
            var partialLoanAmount = Math.Min(quote.Amount, loanAmount);
            return (partialLoanAmount * Math.Pow(1 + (quote.Rate / 12), (TermInMonths))) - partialLoanAmount;
        }

        internal double CalculateInterest(List<Quote> lenderQuotes, double loanAmount)
        {
            var remainingLoan = loanAmount;
            var interest = 0.0;
            foreach (var quote in lenderQuotes)
            {
                interest += CalculateInterest(quote, remainingLoan);
                remainingLoan -= quote.Amount;
                if (remainingLoan < 0)
                    break;
            }
            return interest;
        }

        internal double CalculateApr(double loanAmount, double repaymentAmount)
        {
            var diff = repaymentAmount - loanAmount;
            return (2 * 0.33333333 * diff) * (loanAmount * (TermInMonths + 1));
        }
    }
}

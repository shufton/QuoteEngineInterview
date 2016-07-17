using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using NUnit.Framework;
using ZopaQuoteEngine;

namespace ZopaQuoteEngineTests
{
    [TestFixture]
    public class QuoteCalculationEngineTests
    {
        [Test]
        public void NullQuotesList_ReturnsInvalidQuote()
        {
            var qe = new QuoteCalculationEngine(36);

            var result = qe.RequestQuote(null, 1000.0);

            Assert.False(result.IsValid);
            Assert.AreEqual(QuoteError.NoQuotesAvailable, result.NoQuoteReason);
        }
        [Test]
        public void NoValidQuotes_ReturnsInvalidQuote()
        {
            var qe = new QuoteCalculationEngine(36);

            var result = qe.RequestQuote(new List<Quote>(), 1000.0);

            Assert.False(result.IsValid);
            Assert.AreEqual(QuoteError.NoQuotesAvailable, result.NoQuoteReason);
        }

        [Test]
        public void ArrangeQuotesInOrderOfRate_CorrectlySortsQuoteList()
        {
            var qe = new QuoteCalculationEngine(36);

            var inputQuotes = new List<Quote>
            {
                new Quote("Bob", 0.075, 640),
                new Quote("Jane", 0.069, 480),
                new Quote("Fred", 0.071, 520),
                new Quote("Mary", 0.104, 170),
                new Quote("John", 0.081, 320),
                new Quote("Dave", 0.074, 140),
                new Quote("Angela", 0.071, 60)
            };

            var outputQuotes = qe.ArrangeQuotesInOrderOfRate(inputQuotes);

            var sortedQuotes = new List<Quote>
            {
                new Quote("Jane", 0.069, 480),
                new Quote("Fred", 0.071, 520),
                new Quote("Angela", 0.071, 60),
                new Quote("Dave", 0.074, 140),
                new Quote("Bob", 0.075, 640),
                new Quote("John", 0.081, 320),
                new Quote("Mary", 0.104, 170)
            };

            CollectionAssert.AreEquivalent(sortedQuotes,outputQuotes);
        }

        [Test]
        public void IsCapitalAvailable_CapitalIsAvailable_ReturnsTrue()
        {
            var qe = new QuoteCalculationEngine(36);

            var inputQuotes = new List<Quote>
            {
                new Quote("Bob", 0.075, 640),
                new Quote("Jane", 0.069, 480),
                new Quote("Fred", 0.071, 520),
                new Quote("Mary", 0.104, 170),
                new Quote("John", 0.081, 320),
                new Quote("Dave", 0.074, 140),
                new Quote("Angela", 0.071, 60)
            };

            var ret = qe.IsCapitalAvailable(inputQuotes,1000);
            
            Assert.True(ret);
        }

        [Test]
        public void IsCapitalAvailable_CapitalIsNotAvailable_ReturnsFalse()
        {
            var qe = new QuoteCalculationEngine(36);

            var inputQuotes = new List<Quote>
            {
                new Quote("Bob", 0.075, 640),
                new Quote("Jane", 0.069, 480),
                new Quote("Fred", 0.071, 520),
                new Quote("Mary", 0.104, 170),
                new Quote("John", 0.081, 320),
                new Quote("Dave", 0.074, 140),
                new Quote("Angela", 0.071, 60)
            };

            var ret = qe.IsCapitalAvailable(inputQuotes, 15000);

            Assert.False(ret);
        }

        [Test]
        public void FilterLendersToSatisfyLoanRequest_ReturnsSubsetOfLenders()
        {
            var qe = new QuoteCalculationEngine(36);

            var sortedQuotes = new List<Quote>
            {
                new Quote("Jane", 0.069, 480),
                new Quote("Fred", 0.071, 520),
                new Quote("Angela", 0.071, 60),
                new Quote("Dave", 0.074, 140),
                new Quote("Bob", 0.075, 640),
                new Quote("John", 0.081, 320),
                new Quote("Mary", 0.104, 170)
            };

            var lenderList = qe.FilterLendersToSatisfyLoanRequest(sortedQuotes, 1000.0);

            var lenderQuotes = new List<Quote>
            {
                new Quote("Jane", 0.069, 480),
                new Quote("Fred", 0.071, 520)
            };

            CollectionAssert.AreEqual(lenderQuotes,lenderList);
        }

        [Test]
        public void CalculateRepaymentAmountForQuote_ZeroInterest_FullQuoteAmount()
        {
            var qe = new QuoteCalculationEngine(36);

            var quote = new Quote("Example", 0.0, 1000.0);

            var repayableAmount = qe.CalculateInterest(quote, 1000.0);

            Assert.AreEqual(0.0, repayableAmount, 0.01);
        }

        [Test]
        public void CalculateRepaymentAmountForQuote_FullQuoteAmount()
        {
            var qe = new QuoteCalculationEngine(36);

            var quote = new Quote("Example", 0.07, 1000.0);

            var repayableAmount = qe.CalculateInterest(quote, 1000.0);

            Assert.AreEqual(232.93, repayableAmount, 0.01);
        }

        [Test]
        public void CalculateRepaymentAmountForQuote_PartialQuoteAmount()
        {
            var qe = new QuoteCalculationEngine(36);

            var quote = new Quote("Example", 0.07, 1500.0);

            var repayableAmount = qe.CalculateInterest(quote, 1000.0);

            Assert.AreEqual(232.93, repayableAmount, 0.01);
        }

        [Test]
        public void CalculateInterestFromMultipleLenders_ZeroInterest_ReturnsExpectedValue()
        {
            var qe = new QuoteCalculationEngine(36);

            var lenderQuotes = new List<Quote>
            {
                new Quote("Jane", 0.0, 480),
                new Quote("Fred", 0.0, 520)
            };

            var repayableAmount = qe.CalculateInterest(lenderQuotes, 1000.0);

            Assert.AreEqual(0.0, repayableAmount, 0.01);
        }

        [Test]
        public void CalculateInterestFromMultipleLenders_ReturnsExpectedValue()
        {
            var qe = new QuoteCalculationEngine(36);

            var lenderQuotes = new List<Quote>
            {
                new Quote("Jane", 0.07, 480),
                new Quote("Fred", 0.07, 520)
            };

            var repayableAmount = qe.CalculateInterest(lenderQuotes, 1000.0);

            Assert.AreEqual(232.93, repayableAmount, 0.01);
        }

        [Test]
        public void APRCalculator_CalculatesZeroWhenRepaymentEqualsBorrowed()
        {
            var qe = new QuoteCalculationEngine(36);

            var apr = qe.CalculateApr(1000.0, 1000.0);

            Assert.AreEqual(0.0, apr);
        }

        [Test]
        public void APRCalculator_CalculatesAprForFivePercentAprExample()
        {
            var qe = new QuoteCalculationEngine(36);

            var apr = qe.CalculateApr(1000.0, 1232.93);

            Assert.AreEqual(5.0, apr);
        }
    }
}

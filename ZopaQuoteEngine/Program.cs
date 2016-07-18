using System;

namespace ZopaQuoteEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var inputValidator = new InputValidation();
            if (!inputValidator.ValidateInputs(args))
                return;

            var csvReader = new CsvFileReader(inputValidator.FileName);
            csvReader.Read();

            var quoteTransformer = new QuoteTransformer();
            csvReader.TransformData(quoteTransformer);

            var quoteCalculator = new QuoteCalculationEngine(36);
            var quoteResult = quoteCalculator.RequestQuote(quoteTransformer.Quotes, inputValidator.LoanAmount);

            if (quoteResult.IsValid)
            {
                Console.WriteLine($"Requested Amount: £{quoteResult.Amount}");
                Console.WriteLine($"Rate: {quoteResult.Rate*100:0.#}%");
                Console.WriteLine($"Monthly repayment: £{quoteResult.MontlyRepayment:0.##}");
                Console.WriteLine($"Total repayment: £{quoteResult.TotalRepayment:0.##}");
            }
            else
            {
                Console.WriteLine(QuoteErrorParser.ExplainQuoteError(quoteResult.NoQuoteReason));
            }
        }
    }
}

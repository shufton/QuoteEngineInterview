using System;
using System.Collections.Generic;

namespace ZopaQuoteEngine
{
    internal class QuoteTransformer : ICsvTransformer
    {
        public QuoteTransformer()
        {
            Quotes = new List<Quote>();
        }

        public List<Quote> Quotes { get; private set; }

        public void TransformRow(string[] row)
        {
            if (null == row)
            {
                Console.WriteLine("WARNING: unable to parse invalid quote, input data was null.");
                return;
            }

            if (3 != row.GetLength(0))
            {
                Console.WriteLine("WARNING: unable to parse invalid quote, input data was not in correct format : {0}"
                    , String.Join(",", row));
                return;
            }

            var lender = row[0];
            var rate = Convert.ToDouble(row[1]);
            var amount = Convert.ToDouble(row[2]);

            var quote = new Quote(lender, rate, amount);
            Quotes.Add(quote);
        }
    }
}

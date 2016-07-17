using System;
using System.Diagnostics;

namespace ZopaQuoteEngine
{
    [DebuggerDisplay("Lender:{Lender} Rate:{Rate} Amount:{Amount}")]
    class Quote : IComparable, IComparable<Quote>, IEquatable<Quote>
    {
        public Quote(string lender, double rate, double amount)
        {
            Lender = lender;
            Rate = rate;
            Amount = amount;
        }
        public string Lender { get; private set; }
        public double Rate { get; private set; }
        public double Amount { get; private set; }
        public int CompareTo(object obj)
        {
            if (null == obj)
                throw new ApplicationException("Unable to compare Quote to null object.");

            var quote = obj as Quote;
            if (quote != null)
                return CompareTo(quote);

            throw new ApplicationException($"Unable to Compare Quote Object to an object of type {obj.GetType()}");
        }

        public int CompareTo(Quote other)
        {
            var ret = String.Compare(Lender, other.Lender, StringComparison.Ordinal);
            if (0 == ret)
                ret = Rate.CompareTo(other.Rate);
            if (0 == ret)
                ret = Amount.CompareTo(other.Amount);

            return ret;
        }

        public bool Equals(Quote other)
        {
            return Lender.Equals(other.Lender) &&
                   Rate.Equals(other.Rate) &&
                   Amount.Equals(other.Amount);
        }
    }
}

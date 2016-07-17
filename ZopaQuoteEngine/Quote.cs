namespace ZopaQuoteEngine
{
    class Quote
    {
        public Quote(string lender, decimal rate, decimal amount)
        {
            Lender = lender;
            Rate = rate;
            Amount = amount;
        }
        public string Lender { get; private set; }
        public decimal Rate { get; private set; }
        public decimal Amount { get; private set; }
    }
}

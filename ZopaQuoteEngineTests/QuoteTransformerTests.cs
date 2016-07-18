using System;
using NUnit.Framework;
using ZopaQuoteEngine;

namespace ZopaQuoteEngineTests
{
    [TestFixture]
    public class QuoteTransformerTests
    {
        [Test]
        public void NullInput_IsIgnored()
        {
            var qt = new QuoteTransformer();
            qt.TransformRow(null);

            Assert.AreEqual(0,qt.Quotes.Count);
        }
        [Test]
        public void InvalidColumnCount_IsIgnored()
        {
            var qt = new QuoteTransformer();
            qt.TransformRow(new[] { "col1", "col2" });

            Assert.AreEqual(0, qt.Quotes.Count);
        }
        [Test]
        public void InvalidTypeConversion_IsIgnored()
        {
            var qt = new QuoteTransformer();

            Assert.Throws<FormatException>(()=>qt.TransformRow(new[] { "Lender", "Rate", "Available" }));

            Assert.AreEqual(0, qt.Quotes.Count);
        }
        [Test]
        public void ValidInputData_AddsQuoteToValidQuotesList()
        {
            var qt = new QuoteTransformer();
            qt.TransformRow(new[] { "Bob", "0.075", "640" });

            Assert.AreEqual(1, qt.Quotes.Count);
        }
    }
}

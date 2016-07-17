using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ZopaQuoteEngine;

namespace ZopaQuoteEngineTests
{
    [TestFixture]
    public class InputValidationTests
    {
        [Test]
        public void TooFewProgramArguments_DetectedAsInvalid()
        {
            var iv = new InputValidation();
            Assert.False(iv.ValidateInputs(new[] { "a" }));
        }
        [Test]
        public void TooManyProgramArguments_DetectedAsInvalid()
        {
            var iv = new InputValidation();
            Assert.False(iv.ValidateInputs(new[] { "a", "b", "c" }));
        }
        [Test]
        public void SecondArgumentNotIntegerType_DetectedAsInvalid()
        {
            var iv = new InputValidation();
            Assert.False(iv.ValidateInputs(new[] { "a", "b" }));
        }
        [Test]
        public void LoanRequestIsTooLow_DetectedAsInvalid()
        {
            var iv = new InputValidation();
            Assert.False(iv.ValidateInputs(new[] { "a", "50" }));
        }
        [Test]
        public void LoanRequestIsTooHigh_DetectedAsInvalid()
        {
            var iv = new InputValidation();
            Assert.False(iv.ValidateInputs(new[] { "a", "50000" }));
        }
        [Test]
        public void LoanRequestIsNotMultipleOf100_DetectedAsInvalid()
        {
            var iv = new InputValidation();
            Assert.False(iv.ValidateInputs(new[] { "a", "1234" }));
        }
        [Test]
        public void ValidLoanRequest_DetectedAsValid()
        {
            var iv = new InputValidation();
            Assert.True(iv.ValidateInputs(new[] { "a", "2500" }));
        }
    }
}

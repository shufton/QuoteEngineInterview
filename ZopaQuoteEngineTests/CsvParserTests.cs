using System;
using System.IO;
using NUnit.Framework;
using ZopaQuoteEngine;
using Moq;

namespace ZopaQuoteEngineTests
{
    [TestFixture]
    public class CsvParserTests
    {
        private string TestFile => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "market.csv");

        [Test]
        public void EmptyInput_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new CsvFileReader(String.Empty));
        }

        [Test]
        public void InvalidFilePath_ThrowsArgumentException()
        {
            var randomGuidFileName = "553870C8664E424A91D7613C52B41523";

            Assert.False(File.Exists(randomGuidFileName),"Test depends on a missing file, but that file seems to exist.");
            Assert.Throws<ArgumentException>(() => new CsvFileReader(String.Empty));
        }

        [Test]
        public void ValidFileName_ContinuesWithNoError()
        {
            Assert.DoesNotThrow(() => new CsvFileReader(TestFile));
        }

        [Test]
        public void NullStream_ThrowsExceptionAndRetainsEmptyResults()
        {
            var csv = new CsvFileReader(TestFile);

            Assert.Throws<ArgumentException>(() => csv.ReadImpl(null));

            Assert.IsEmpty(csv.RawData);
        }

        [Test]
        public void EmptyStreamType1_RetainsEmptyResults()
        {
            var csv = new CsvFileReader(TestFile);
            var sr = new StringReader("");

            csv.ReadImpl(sr);

            Assert.IsEmpty(csv.RawData);
        }

        [Test]
        public void EmptyStreamType2_RetainsEmptyResults()
        {
            var csv = new CsvFileReader(TestFile);
            var sr = new StringReader("\r\n\r\n\r\n");

            csv.ReadImpl(sr);

            Assert.IsEmpty(csv.RawData);
        }

        [Test]
        public void HeaderOnly_RetainsEmptyResults()
        {
            var csv = new CsvFileReader(TestFile);
            var sr = new StringReader("Lender,Rate,Available\r\n\r\n\r\n");

            csv.ReadImpl(sr);

            Assert.IsEmpty(csv.RawData);
        }

        [Test]
        public void SingleRowValidInput_ParsedToRawData()
        {
            var csv = new CsvFileReader(TestFile);
            var sr = new StringReader("Lender,Rate,Available\r\nBob,0.075,640");

            csv.ReadImpl(sr);

            Assert.AreEqual(1,csv.RawData.Count);
            Assert.AreEqual(3,csv.RawData[0].GetLength(0));
        }

        [Test]
        public void MultipleRowValidData_ParsedToRawData()
        {
            var csv = new CsvFileReader(TestFile);
            var sr = new StringReader("Lender,Rate,Available\r\nBob,0.075,640\r\nJane,0.069,480\r\nFred,0.071,520\r\nMary,0.104,170\r\nJohn,0.081,320\r\nDave,0.074,140\r\nAngela,0.071,60");

            csv.ReadImpl(sr);

            Assert.AreEqual(7, csv.RawData.Count);
            for(var i=0;i<7;++i)
                Assert.AreEqual(3, csv.RawData[i].GetLength(0));
        }

        [Test]
        public void NullTransformer_DoesntCauseException()
        {
            var csv = new CsvFileReader(TestFile);
            var sr = new StringReader("Lender,Rate,Available\r\nBob,0.075,640\r\nJane,0.069,480\r\nFred,0.071,520\r\nMary,0.104,170\r\nJohn,0.081,320\r\nDave,0.074,140\r\nAngela,0.071,60");

            csv.ReadImpl(sr);
            ICsvTransformer ts = null;

            Assert.DoesNotThrow(() => csv.TransformData(ts));
        }

        [Test]
        public void TransformData_CalledForEachRow()
        {
            var csv = new CsvFileReader(TestFile);
            var sr = new StringReader("Lender,Rate,Available\r\nBob,0.075,640\r\nJane,0.069,480\r\nFred,0.071,520\r\nMary,0.104,170\r\nJohn,0.081,320\r\nDave,0.074,140\r\nAngela,0.071,60");

            csv.ReadImpl(sr);
            var mockTransformer = new Mock<ICsvTransformer>();
            mockTransformer.Setup(foo => foo.TransformRow(It.IsAny<string[]>()));

            Assert.DoesNotThrow(() => csv.TransformData(mockTransformer.Object));

            mockTransformer.Verify(foo =>foo.TransformRow(It.IsAny<string[]>()),Times.Exactly(7));
        }
    }
}

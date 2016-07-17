using System;
using System.Collections.Generic;
using System.IO;

namespace ZopaQuoteEngine
{
    /// <summary>
    /// The CsvFile Reader has the following assumptions
    /// - Simple csv format (i.e. no escaped cells containing additional commas, crlf etc
    /// - Contains a header line at the beginning which will be skipped on load
    /// </summary>
    internal class CsvFileReader
    {
        public string Filename { get; }
        public List<String[]> RawData { get; }

        public CsvFileReader(string fullFilePath)
        {
            if (String.IsNullOrEmpty(fullFilePath))
                throw new ArgumentException("fullFilePath parameter must be supplied", nameof(fullFilePath));

            if (!File.Exists(fullFilePath))
                throw new ArgumentException("fullFilePath parameter must refer to a valid file");

            Filename = fullFilePath;
            RawData = new List<string[]>();
        }
        public void Read()
        {
            using (var tr = new StreamReader(File.Open(Filename, FileMode.Open, FileAccess.Read)))
                ReadImpl(tr);
        }

        internal void ReadImpl(TextReader reader)
        {
            if (null == reader)
                throw new ArgumentException("Cannot read csv file from a null input stream", nameof(reader));

            // Skip the first line as part of the header row assumption.
            reader.ReadLine();
            var line = reader.ReadLine();
            while (null != line)
            {
                if (!String.IsNullOrEmpty(line))
                {
                    RawData.Add(line.Split(','));
                }
                line = reader.ReadLine();
            }
        }
    }
}

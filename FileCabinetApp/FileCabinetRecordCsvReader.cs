using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using static FileCabinetApp.ConstantsAndValidationRulesSettings.Constants;

namespace FileCabinetApp
{
    /// <summary>Provides method to import <see cref="FileCabinetRecord"/> records from csv file.</summary>
    public class FileCabinetRecordCsvReader
    {
        private readonly StreamReader reader;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.</summary>
        /// <param name="reader">Reader.</param>
        /// <exception cref="ArgumentNullException">Thrown when reader
        /// is null.</exception>
        public FileCabinetRecordCsvReader(StreamReader reader)
        {
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        /// <summary>Reads all records from csv file.</summary>
        /// <returns>Returns read records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            var records = new List<FileCabinetRecord>();
            this.reader.ReadLine();
            while (!this.reader.EndOfStream)
            {
                var recordString = this.reader.ReadLine().Split(CsvFileSeparator, StringSplitOptions.RemoveEmptyEntries);
                var record = new FileCabinetRecord
                {
                    Id = int.Parse(recordString[0], CultureInfo.InvariantCulture),
                    Name = new FullName(recordString[1], recordString[2]),
                    DateOfBirth = DateTime.Parse(recordString[3], CultureInfo.InvariantCulture),
                    Sex = char.Parse(recordString[4]),
                    NumberOfReviews = short.Parse(recordString[5], CultureInfo.InvariantCulture),
                    Salary = decimal.Parse(recordString[6], CultureInfo.InvariantCulture),
                };
                records.Add(record);
            }

            return records;
        }
    }
}

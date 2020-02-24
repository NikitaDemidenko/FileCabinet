using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using static FileCabinetApp.Constants;

namespace FileCabinetApp
{
    /// <summary>Provides method for saving snapshots to csv file.</summary>
    public class FileCabinetServiceSnapshot
    {
        private readonly List<Tuple<FileCabinetRecord, string>> invalidRecords = new List<Tuple<FileCabinetRecord, string>>();
        private readonly List<FileCabinetRecord> records;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.</summary>
        public FileCabinetServiceSnapshot()
        {
            this.records = new List<FileCabinetRecord>();
        }

        /// <summary>Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.</summary>
        /// <param name="records">Records to save.</param>
        /// <exception cref="ArgumentNullException">Thrown when records is null.</exception>
        public FileCabinetServiceSnapshot(ReadOnlyCollection<FileCabinetRecord> records)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            this.records = new List<FileCabinetRecord>(records);
        }

        /// <summary>Gets the records.</summary>
        /// <value>The records.</value>
        public ReadOnlyCollection<FileCabinetRecord> Records => new ReadOnlyCollection<FileCabinetRecord>(this.records);

        /// <summary>Gets the invalid records of taken snapshot.</summary>
        /// <value>Invalid records of taken snapshot.</value>
        public ReadOnlyCollection<Tuple<FileCabinetRecord, string>> InvalidRecords => new ReadOnlyCollection<Tuple<FileCabinetRecord, string>>(this.invalidRecords);

        /// <summary>Saves snapshot to csv file.</summary>
        /// <param name="writer">Writer.</param>
        /// <exception cref="ArgumentNullException">Thrown when writer is null.</exception>
        public void SaveToCsv(StreamWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var csvWriter = new FileCabinetRecordCsvWriter(writer);
            writer.WriteLine("Id;First Name;Last Name;Date of Birth;Sex;Number of Reviews;Salary");
            foreach (var record in this.records)
            {
                csvWriter.Write(record);
            }
        }

        /// <summary>Saves snapshot to xml file.</summary>
        /// <param name="writer">Writer.</param>
        /// <exception cref="ArgumentNullException">Thrown when writer is null.</exception>
        public void SaveToXml(StreamWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var settings = new XmlWriterSettings
            {
                Indent = true,
            };
            var formatter = new XmlSerializer(typeof(List<FileCabinetRecord>));
            var xmlWriter = new FileCabinetRecordXmlWriter(writer);
            xmlWriter.Write(this.records, formatter);
        }

        /// <summary>Loads records from CSV file.</summary>
        /// <param name="reader">Reader.</param>
        /// <param name="validator">Validator.</param>
        /// <exception cref="ArgumentNullException">Thrown when reader or validator is null.</exception>
        public void LoadFromCsv(StreamReader reader, IRecordValidator validator)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (validator == null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            var csvReader = new FileCabinetRecordCsvReader(reader);
            IList<FileCabinetRecord> records;
            try
            {
                records = csvReader.ReadAll();
            }
            catch (FormatException)
            {
                throw;
            }
            catch (IndexOutOfRangeException)
            {
                throw;
            }

            UnverifiedData unverifiedData;
            foreach (var record in records)
            {
                unverifiedData = new UnverifiedData(record);
                try
                {
                    validator.ValidateParameters(unverifiedData);
                }
                catch (ArgumentException ex)
                {
                    this.invalidRecords.Add(new Tuple<FileCabinetRecord, string>(record, ex.Message));
                    continue;
                }

                this.records.Add(record);
            }
        }

        /// <summary>Loads records from XML file.</summary>
        /// <param name="reader">Reader.</param>
        /// <param name="validator">Validator.</param>
        /// <exception cref="ArgumentNullException">Thrown when reader or validator is null.</exception>
        public void LoadFromXml(XmlReader reader, IRecordValidator validator)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (validator == null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            var xmlReader = new FileCabinetRecordXmlReader(reader);
            IList<FileCabinetRecord> records;
            try
            {
                records = xmlReader.ReadAll();
            }
            catch (InvalidOperationException)
            {
                throw;
            }

            UnverifiedData unverifiedData;
            foreach (var record in records)
            {
                unverifiedData = new UnverifiedData(record);
                try
                {
                    validator.ValidateParameters(unverifiedData);
                }
                catch (ArgumentException ex)
                {
                    this.invalidRecords.Add(new Tuple<FileCabinetRecord, string>(record, ex.Message));
                    continue;
                }

                this.records.Add(record);
            }
        }
    }
}

using System;
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
        private readonly FileCabinetRecord[] records;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.</summary>
        /// <param name="records">Records to save.</param>
        /// <exception cref="ArgumentNullException">Thrown when records is null.</exception>
        public FileCabinetServiceSnapshot(ReadOnlyCollection<FileCabinetRecord> records)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            this.records = new FileCabinetRecord[records.Count];
            records.CopyTo(this.records, FirstElementIndex);
        }

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
            var formatter = new XmlSerializer(typeof(FileCabinetRecord[]));
            var xmlWriter = new FileCabinetRecordXmlWriter(writer);
            xmlWriter.Write(this.records, formatter);
        }
    }
}

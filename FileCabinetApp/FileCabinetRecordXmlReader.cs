using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>Provides method to import <see cref="FileCabinetRecord"/> records from xml file.</summary>
    public class FileCabinetRecordXmlReader
    {
        private readonly XmlReader reader;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.</summary>
        /// <param name="reader">Reader.</param>
        /// <exception cref="ArgumentNullException">Thrown when reader is null.</exception>
        public FileCabinetRecordXmlReader(XmlReader reader)
        {
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        /// <summary>Reads all records from xml file.</summary>
        /// <returns>Returns read records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            var xmlReader = new XmlSerializer(typeof(List<FileCabinetRecord>));
            var records = (List<FileCabinetRecord>)xmlReader.Deserialize(this.reader);

            return records;
        }
    }
}

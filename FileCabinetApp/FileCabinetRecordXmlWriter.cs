using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>Provides method for saving <see cref="FileCabinetRecord"/> to xml file.</summary>
    public class FileCabinetRecordXmlWriter
    {
        private readonly XmlWriter writer;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.</summary>
        /// <param name="writer">Xml writer.</param>
        /// <exception cref="ArgumentNullException">Thrown when writer is null.</exception>
        public FileCabinetRecordXmlWriter(XmlWriter writer)
        {
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        /// <summary>Writes the specified record to xml file.</summary>
        /// <param name="record">Record.</param>
        /// <exception cref="ArgumentNullException">Thrown when record is null.</exception>
        public void Write(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            this.writer.WriteStartElement("record");
            this.writer.WriteAttributeString("id", $"{record.Id}");

            this.writer.WriteStartElement("name");
            this.writer.WriteAttributeString("first", $"{record.FirstName}");
            this.writer.WriteAttributeString("last", $"{record.LastName}");
            this.writer.WriteEndElement();

            this.writer.WriteElementString("dateOfBirth", $"{record.DateOfBirth:MM/dd/yyyy}");
            this.writer.WriteElementString("sex", $"{record.Sex}");
            this.writer.WriteElementString("numberOfReviews", $"{record.NumberOfReviews}");
            this.writer.WriteElementString("salary", $"{record.Salary}");

            this.writer.WriteEndElement();
        }
    }
}

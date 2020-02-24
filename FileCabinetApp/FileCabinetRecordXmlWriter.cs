using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>Provides method for saving <see cref="FileCabinetRecord"/> to xml file.</summary>
    public class FileCabinetRecordXmlWriter
    {
        private readonly StreamWriter writer;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.</summary>
        /// <param name="writer">Stream writer.</param>
        /// <exception cref="ArgumentNullException">Thrown when writer is null.</exception>
        public FileCabinetRecordXmlWriter(StreamWriter writer)
        {
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        /// <summary>Writes the specified record to xml file.</summary>
        /// <param name="records">Records.</param>
        /// <param name="formatter">Xml formatter.</param>
        /// <exception cref="ArgumentNullException">Thrown when records or formatter is null.</exception>
        public void Write(List<FileCabinetRecord> records, XmlSerializer formatter)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            formatter.Serialize(this.writer, records);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
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
        /// <param name="streamWriter">Stream writer.</param>
        /// <exception cref="ArgumentNullException">Thrown when streamWriter is null.</exception>
        public void SaveToCsv(StreamWriter streamWriter)
        {
            if (streamWriter == null)
            {
                throw new ArgumentNullException(nameof(streamWriter));
            }

            var csvWriter = new FileCabinetRecordCsvWriter(streamWriter);
            streamWriter.WriteLine("Id;First Name;Last Name;Date of Birth;Sex;Number of Reviews;Salary");
            foreach (var record in this.records)
            {
                csvWriter.Write(record);
            }
        }
    }
}

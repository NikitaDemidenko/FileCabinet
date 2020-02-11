using System;
using System.IO;

namespace FileCabinetApp
{
    /// <summary>Provides method for saving <see cref="FileCabinetRecord"/> to csv file.</summary>
    public class FileCabinetRecordCsvWriter
    {
        private readonly TextWriter writer;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.</summary>
        /// <param name="writer">Text writer.</param>
        /// <exception cref="ArgumentNullException">Thrown when writer is null.</exception>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        /// <summary>Writes the specified record to csv file.</summary>
        /// <param name="record">Record.</param>
        /// <exception cref="ArgumentNullException">Thrown when record is null.</exception>
        public void Write(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            this.writer.Write($"{record.Id};");
            this.writer.Write($"{record.FirstName};");
            this.writer.Write($"{record.LastName};");
            this.writer.Write($"{record.DateOfBirth:MM/dd/yyyy};");
            this.writer.Write($"{record.Sex};");
            this.writer.Write($"{record.NumberOfReviews};");
            this.writer.WriteLine($"{record.Salary};");
        }
    }
}

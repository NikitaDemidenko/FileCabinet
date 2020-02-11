using System;
using System.IO;

namespace FileCabinetApp
{
    /// <summary>Provides method for saving <see cref="FileCabinetRecord"/> to csv file.</summary>
    public class FileCabinetRecordCsvWriter
    {
        private readonly TextWriter textWriter;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.</summary>
        /// <param name="textWriter">Text writer.</param>
        /// <exception cref="ArgumentNullException">Thrown when textWriter is null.</exception>
        public FileCabinetRecordCsvWriter(TextWriter textWriter)
        {
            this.textWriter = textWriter ?? throw new ArgumentNullException(nameof(textWriter));
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

            this.textWriter.Write($"{record.Id};");
            this.textWriter.Write($"{record.FirstName};");
            this.textWriter.Write($"{record.LastName};");
            this.textWriter.Write($"{record.DateOfBirth:MM/dd/yyyy};");
            this.textWriter.Write($"{record.Sex};");
            this.textWriter.Write($"{record.NumberOfReviews};");
            this.textWriter.WriteLine($"{record.Salary};");
        }
    }
}

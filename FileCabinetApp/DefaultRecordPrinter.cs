using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>Default record printer.</summary>
    /// <seealso cref="IRecordPrinter" />
    public class DefaultRecordPrinter : IRecordPrinter
    {
        /// <summary>Prints the specified records.</summary>
        /// <param name="records">The records.</param>
        /// <exception cref="ArgumentNullException">Thrown when records
        /// is null.</exception>
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            foreach (var record in records)
            {
                Console.WriteLine(record);
            }
        }
    }
}

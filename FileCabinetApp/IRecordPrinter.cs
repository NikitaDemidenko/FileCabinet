using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>Record printer interface.</summary>
    public interface IRecordPrinter
    {
        /// <summary>Prints the specified records.</summary>
        /// <param name="records">The records.</param>
        public void Print(IEnumerable<FileCabinetRecord> records);
    }
}

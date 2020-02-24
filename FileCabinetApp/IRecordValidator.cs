using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    ///   <para>Provides functionality to validate parameters.</para>
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>Validates user input data.</summary>
        /// <param name="unverifiedData">Raw data.</param>
        public void ValidateParameters(UnverifiedData unverifiedData);
    }
}

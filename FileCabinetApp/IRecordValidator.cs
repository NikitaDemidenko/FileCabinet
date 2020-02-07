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
        /// <param name="userInputData">User input data.</param>
        public void ValidateParameters(UserInputData userInputData);
    }
}

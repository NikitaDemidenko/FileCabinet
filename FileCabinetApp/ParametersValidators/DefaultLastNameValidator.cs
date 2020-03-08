using System;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Default last name validator.</summary>
    /// <seealso cref="IRecordValidator" />
    public class DefaultLastNameValidator : IRecordValidator
    {
        /// <summary>Validates user input data.</summary>
        /// <param name="parameters">Parameters to validate.</param>
        /// <exception cref="ArgumentException">Last name is empty
        /// or
        /// Last name's length is out of range
        /// or
        /// parameters isn't string.</exception>
        public void ValidateParameters(object parameters)
        {
            if (parameters is string lastName)
            {
                if (string.IsNullOrWhiteSpace(lastName))
                {
                    throw new ArgumentException("Last name is empty.");
                }

                if (lastName.Length < MinNumberOfSymbols || lastName.Length > MaxNumberOfSymbols)
                {
                    throw new ArgumentException("Last name's length is out of range.");
                }
            }
            else
            {
                throw new ArgumentException($"{nameof(parameters)} must be string.");
            }
        }
    }
}

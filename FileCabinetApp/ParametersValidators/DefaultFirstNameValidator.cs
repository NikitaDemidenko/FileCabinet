using System;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Default first name validator.</summary>
    /// <seealso cref="IRecordValidator" />
    public class DefaultFirstNameValidator : IRecordValidator
    {
        /// <summary>Validates user input data.</summary>
        /// <param name="parameters">Parameters to validate.</param>
        /// <exception cref="ArgumentException">First name is empty
        /// or
        /// First name's length is out of range or parameters isn't string.</exception>
        public void ValidateParameters(object parameters)
        {
            if (parameters is string firstName)
            {
                if (string.IsNullOrWhiteSpace(firstName))
                {
                    throw new ArgumentException("First name is empty.");
                }

                if (firstName.Length < MinNumberOfSymbols || firstName.Length > MaxNumberOfSymbols)
                {
                    throw new ArgumentException("First name's length is out of range.");
                }
            }
            else
            {
                throw new ArgumentException($"{nameof(parameters)} must be string.");
            }
        }
    }
}

using System;
using System.Text.RegularExpressions;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Custom last name validator.</summary>
    /// <seealso cref="IRecordValidator" />
    public class CustomLastNameValidator : IRecordValidator
    {
        /// <summary>Validates user input data.</summary>
        /// <param name="parameters">Parameters to validate.</param>
        /// <exception cref="ArgumentException">Last name is empty
        /// or
        /// Last name's length is out of range or has invalid characters
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

                if (lastName.Length < MinNumberOfSymbols || lastName.Length > MaxNumberOfSymbols ||
                    !Regex.IsMatch(lastName, AllowedCharacters))
                {
                    throw new ArgumentException("Last name's length is out of range or has invalid characters.");
                }
            }
            else
            {
                throw new ArgumentException($"{nameof(parameters)} must be string.");
            }
        }
    }
}

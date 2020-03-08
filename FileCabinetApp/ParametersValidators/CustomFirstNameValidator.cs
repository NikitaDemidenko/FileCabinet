using System;
using System.Text.RegularExpressions;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Custom first name validator.</summary>
    /// <seealso cref="IRecordValidator" />
    public class CustomFirstNameValidator : IRecordValidator
    {
        /// <summary>Validates user input data.</summary>
        /// <param name="parameters">Parameters to validate.</param>
        /// <exception cref="ArgumentException">First name is empty
        /// or
        /// First name's length is out of range or has invalid characters or parameters isn't string.</exception>
        public void ValidateParameters(object parameters)
        {
            if (parameters is string firstName)
            {
                if (string.IsNullOrWhiteSpace(firstName))
                {
                    throw new ArgumentException("First name is empty.");
                }

                if (firstName.Length < MinNumberOfSymbols || firstName.Length > MaxNumberOfSymbols ||
                    !Regex.IsMatch(firstName, AllowedCharacters))
                {
                    throw new ArgumentException("First name's length is out of range or has invalid characters.");
                }
            }
            else
            {
                throw new ArgumentException($"{nameof(parameters)} must be string.");
            }
        }
    }
}

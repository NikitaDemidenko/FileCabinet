using System;
using System.Text.RegularExpressions;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>First name validator.</summary>
    /// <seealso cref="IRecordValidator" />
    public class FirstNameValidator : IRecordValidator
    {
        private int minLength;
        private int maxLength;
        private bool isCustomValidationRules;

        /// <summary>Initializes a new instance of the <see cref="FirstNameValidator"/> class.</summary>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="isCustomValidationRules">Determines wether validation rules are custom.</param>
        public FirstNameValidator(int minLength, int maxLength, bool isCustomValidationRules)
        {
            if (minLength < 0)
            {
                throw new ArgumentException($"Length cannot be negative.");
            }

            if (maxLength > MaxNumberOfSymbols)
            {
                throw new ArgumentException($"{nameof(maxLength)} cannot be greater than MaxNumberOfSymbols.");
            }

            if (minLength > maxLength)
            {
                throw new ArgumentException($"{nameof(minLength)} cannot be greater than {nameof(maxLength)}.");
            }

            this.minLength = minLength;
            this.maxLength = maxLength;
            this.isCustomValidationRules = isCustomValidationRules;
        }

        /// <summary>Validates user input data.</summary>
        /// <param name="data">Data to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="ArgumentException">First name is empty
        /// or
        /// First name's length is out of range or has invalid characters .</exception>
        public void ValidateParameters(UnverifiedData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (string.IsNullOrWhiteSpace(data.FirstName))
            {
                throw new ArgumentException("First name is empty.");
            }

            if (data.FirstName.Length < this.minLength || data.FirstName.Length > this.maxLength)
            {
                throw new ArgumentException("First name's length is out of range.");
            }

            if (this.isCustomValidationRules)
            {
                if (!Regex.IsMatch(data.FirstName, AllowedCharacters))
                {
                    throw new ArgumentException("First name has invalid characters.");
                }
            }
        }
    }
}

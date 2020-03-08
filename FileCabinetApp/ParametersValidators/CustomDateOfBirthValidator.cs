using System;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Custom date of birth validator.</summary>
    /// <seealso cref="IRecordValidator" />
    public class CustomDateOfBirthValidator : IRecordValidator
    {
        /// <summary>Validates user input data.</summary>
        /// <param name="parameters">Parameters to validate.</param>
        /// <exception cref="ArgumentException">Invalid date of birth or
        /// parameters isn't DateTime.</exception>
        public void ValidateParameters(object parameters)
        {
            if (parameters is DateTime dateOfBirth)
            {
                if (dateOfBirth < MinDateOfBirth || dateOfBirth >= DateTime.Now)
                {
                    throw new ArgumentException("Invalid date of birth.");
                }
            }
            else
            {
                throw new ArgumentException($"{nameof(parameters)} must be DateTime.");
            }
        }
    }
}

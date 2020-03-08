using System;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Default salary validator.</summary>
    /// <seealso cref="IRecordValidator" />
    public class DefaultSalaryValidator : IRecordValidator
    {
        /// <summary>Validates user input data.</summary>
        /// <param name="parameters">Parameters to validate.</param>
        /// <exception cref="ArgumentException">Salary is less than zero
        /// or
        /// parameters isn't decimal.</exception>
        public void ValidateParameters(object parameters)
        {
            if (parameters is decimal salary)
            {
                if (salary < MinValueOfSalary)
                {
                    throw new ArgumentException("Salary cannot be less than zero.");
                }
            }
            else
            {
                throw new ArgumentException($"{nameof(parameters)} must be decimal.");
            }
        }
    }
}

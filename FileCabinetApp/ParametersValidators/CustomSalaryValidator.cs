using System;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Custom salary validator.</summary>
    /// <seealso cref="IRecordValidator" />
    public class CustomSalaryValidator : IRecordValidator
    {
        /// <summary>Validates user input data.</summary>
        /// <param name="parameters">Parameters to validate.</param>
        /// <exception cref="ArgumentException">Salary is less than MinValueOfSalaryCustom
        /// or
        /// parameters isn't decimal.</exception>
        public void ValidateParameters(object parameters)
        {
            if (parameters is decimal salary)
            {
                if (salary < MinValueOfSalaryCustom)
                {
                    throw new ArgumentException("Salary is too small.");
                }
            }
            else
            {
                throw new ArgumentException($"{nameof(parameters)} must be decimal.");
            }
        }
    }
}

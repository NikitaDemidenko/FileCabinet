using System;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Salary validator.</summary>
    /// <seealso cref="IRecordValidator" />
    public class SalaryValidator : IRecordValidator
    {
        private decimal minSalary;

        /// <summary>Initializes a new instance of the <see cref="SalaryValidator"/> class.</summary>
        /// <param name="minSalary">The minimum salary.</param>
        public SalaryValidator(decimal minSalary)
        {
            if (minSalary < 0)
            {
                throw new ArgumentException($"{nameof(minSalary)} cannot be negative.");
            }

            this.minSalary = minSalary;
        }

        /// <summary>Validates user input data.</summary>
        /// <param name="parameters">Parameters to validate.</param>
        /// <exception cref="ArgumentException">Salary is less than MinValueOfSalaryCustom
        /// or
        /// parameters isn't decimal.</exception>
        public void ValidateParameters(object parameters)
        {
            if (parameters is decimal salary)
            {
                if (salary < this.minSalary)
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

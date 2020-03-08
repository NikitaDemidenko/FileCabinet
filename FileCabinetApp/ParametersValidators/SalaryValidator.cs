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
        /// <param name="data">Data to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="ArgumentException">Salary is less than specified value.</exception>
        public void ValidateParameters(UnverifiedData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Salary < this.minSalary)
            {
                throw new ArgumentException("Salary is too small.");
            }
        }
    }
}

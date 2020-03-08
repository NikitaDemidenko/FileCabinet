using System;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Date of birth validator.</summary>
    /// <seealso cref="IRecordValidator" />
    public class DateOfBirthValidator : IRecordValidator
    {
        private DateTime from;
        private DateTime to;

        /// <summary>Initializes a new instance of the <see cref="DateOfBirthValidator"/> class.</summary>
        /// <param name="from">From date.</param>
        /// <param name="to">To date.</param>
        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            if (from > to)
            {
                throw new ArgumentException($"{nameof(from)} cannot be greater than {nameof(to)}.");
            }

            this.from = from;
            this.to = to;
        }

        /// <summary>Validates user input data.</summary>
        /// <param name="data">Data to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="ArgumentException">Invalid date of birth.</exception>
        public void ValidateParameters(UnverifiedData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.DateOfBirth < this.from || data.DateOfBirth >= this.to)
            {
                throw new ArgumentException("Invalid date of birth.");
            }
        }
    }
}

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
        /// <param name="parameters">Parameters to validate.</param>
        /// <exception cref="ArgumentException">Invalid date of birth or
        /// parameters isn't DateTime.</exception>
        public void ValidateParameters(object parameters)
        {
            if (parameters is DateTime dateOfBirth)
            {
                if (dateOfBirth < this.from || dateOfBirth >= this.to)
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

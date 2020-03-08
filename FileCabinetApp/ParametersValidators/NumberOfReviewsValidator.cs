using System;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Number of reviews validator.</summary>
    /// <seealso cref="IRecordValidator" />
    public class NumberOfReviewsValidator : IRecordValidator
    {
        private short minNumber;

        /// <summary>Initializes a new instance of the <see cref="NumberOfReviewsValidator"/> class.</summary>
        /// <param name="minNumber">The minimum number of reviews.</param>
        public NumberOfReviewsValidator(short minNumber)
        {
            if (minNumber < 0)
            {
                throw new ArgumentException($"Number cannot be negative");
            }

            this.minNumber = minNumber;
        }

        /// <summary>Validates user input data.</summary>
        /// <param name="data">Data to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="ArgumentException">Number of reviews is less than specified number.</exception>
        public void ValidateParameters(UnverifiedData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.NumberOfReviews < this.minNumber)
            {
                throw new ArgumentException("Number of reviews is too small.");
            }
        }
    }
}

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
        /// <param name="parameters">Parameters to validate.</param>
        /// <exception cref="ArgumentException">Number of reviews is less than MinNumberOfReviewsCustom
        /// or
        /// parameters isn't short.</exception>
        public void ValidateParameters(object parameters)
        {
            if (parameters is short numberOfReviews)
            {
                if (numberOfReviews < this.minNumber)
                {
                    throw new ArgumentException("Number of reviews is too small.");
                }
            }
            else
            {
                throw new ArgumentException($"{nameof(parameters)} must be short.");
            }
        }
    }
}

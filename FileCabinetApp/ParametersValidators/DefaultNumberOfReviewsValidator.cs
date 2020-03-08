using System;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Default number of reviews validator.</summary>
    /// <seealso cref="IRecordValidator" />
    public class DefaultNumberOfReviewsValidator : IRecordValidator
    {
        /// <summary>Validates user input data.</summary>
        /// <param name="parameters">Parameters to validate.</param>
        /// <exception cref="ArgumentException">Number of reviews is less than zero
        /// or
        /// parameters isn't short.</exception>
        public void ValidateParameters(object parameters)
        {
            if (parameters is short numberOfReviews)
            {
                if (numberOfReviews < MinNumberOfReviews)
                {
                    throw new ArgumentException("Number of reviews cannot be less than zero.");
                }
            }
            else
            {
                throw new ArgumentException($"{nameof(parameters)} must be short.");
            }
        }
    }
}

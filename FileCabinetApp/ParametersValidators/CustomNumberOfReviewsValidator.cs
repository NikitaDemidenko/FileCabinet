using System;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Custom number of reviews validator.</summary>
    /// <seealso cref="IRecordValidator" />
    public class CustomNumberOfReviewsValidator : IRecordValidator
    {
        /// <summary>Validates user input data.</summary>
        /// <param name="parameters">Parameters to validate.</param>
        /// <exception cref="ArgumentException">Number of reviews is less than MinNumberOfReviewsCustom
        /// or
        /// parameters isn't short.</exception>
        public void ValidateParameters(object parameters)
        {
            if (parameters is short numberOfReviews)
            {
                if (numberOfReviews < MinNumberOfReviewsCustom)
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

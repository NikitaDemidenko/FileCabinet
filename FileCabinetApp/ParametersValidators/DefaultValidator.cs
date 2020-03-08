using System;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Performs default validation on user input data.</summary>
    /// <seealso cref="IRecordValidator" />
    public sealed class DefaultValidator : IRecordValidator
    {
        /// <summary>Validates user input parameters using default validation rules.</summary>
        /// <param name="parameters">Raw data.</param>
        /// <exception cref="ArgumentNullException">Thrown when <em>parameters</em> is <em>null</em>.</exception>
        /// <exception cref="ArgumentException">Thrown when one of the user input parameters is invalid.</exception>
        public void ValidateParameters(object parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters is UnverifiedData data)
            {
                new DefaultFirstNameValidator().ValidateParameters(data.FirstName);
                new DefaultLastNameValidator().ValidateParameters(data.LastName);
                new DefaultDateOfBirthValidator().ValidateParameters(data.DateOfBirth);
                new DefaultSexValidator().ValidateParameters(data.Sex);
                new DefaultNumberOfReviewsValidator().ValidateParameters(data.NumberOfReviews);
                new DefaultSalaryValidator().ValidateParameters(data.Salary);
            }
            else
            {
                throw new ArgumentException($"{nameof(parameters)} must be UnverifiedData.");
            }
        }
    }
}

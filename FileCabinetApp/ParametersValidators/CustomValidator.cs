using System;
using System.Text.RegularExpressions;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Performs custom validation on user input data.</summary>
    /// <seealso cref="IRecordValidator" />
    public sealed class CustomValidator : IRecordValidator
    {
        /// <summary>Validates user input parameters using custom validation rules.</summary>
        /// <param name="parameters">Raw data.</param>
        /// <exception cref="ArgumentNullException">Thrown when <em>userInput</em> is <em>null</em>.</exception>
        /// <exception cref="ArgumentException">Thrown when one of the user input parameters is invalid.</exception>
        public void ValidateParameters(object parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters is UnverifiedData data)
            {
                new CustomFirstNameValidator().ValidateParameters(data.FirstName);
                new CustomLastNameValidator().ValidateParameters(data.LastName);
                new CustomDateOfBirthValidator().ValidateParameters(data.DateOfBirth);
                new CustomSexValidator().ValidateParameters(data.Sex);
                new CustomNumberOfReviewsValidator().ValidateParameters(data.NumberOfReviews);
                new CustomSalaryValidator().ValidateParameters(data.Salary);
            }
            else
            {
                throw new ArgumentException($"{nameof(parameters)} must be UnverifiedData.");
            }
        }
    }
}

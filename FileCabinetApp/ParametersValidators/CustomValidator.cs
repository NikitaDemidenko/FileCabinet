using System;
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
                new FirstNameValidator(MinNumberOfSymbols, MaxNumberOfSymbols, true).ValidateParameters(data.FirstName);
                new LastNameValidator(MinNumberOfSymbols, MaxNumberOfSymbols, true).ValidateParameters(data.LastName);
                new DateOfBirthValidator(MinDateOfBirthCustom, DateTime.Now).ValidateParameters(data.DateOfBirth);
                new SexValidator().ValidateParameters(data.Sex);
                new NumberOfReviewsValidator(MinNumberOfReviewsCustom).ValidateParameters(data.NumberOfReviews);
                new SalaryValidator(MinValueOfSalaryCustom).ValidateParameters(data.Salary);
            }
            else
            {
                throw new ArgumentException($"{nameof(parameters)} must be UnverifiedData.");
            }
        }
    }
}

using System;
using System.Text.RegularExpressions;
using static FileCabinetApp.Constants;

namespace FileCabinetApp
{
    /// <summary>Performs custom validation on user input data.</summary>
    /// <seealso cref="IRecordValidator" />
    public sealed class CustomValidator : IRecordValidator
    {
        /// <summary>Validates user input parameters using custom validation rules.</summary>
        /// <param name="userInputData">User input.</param>
        /// <exception cref="ArgumentNullException">Thrown when <em>userInput</em> is <em>null</em>.</exception>
        /// <exception cref="ArgumentException">Thrown when one of the user input parameters is invalid.</exception>
        public void ValidateParameters(UserInputData userInputData)
        {
            if (userInputData == null)
            {
                throw new ArgumentNullException(nameof(userInputData));
            }

            if (string.IsNullOrWhiteSpace(userInputData.FirstName))
            {
                throw new ArgumentException("First name is empty.");
            }

            if (string.IsNullOrWhiteSpace(userInputData.LastName))
            {
                throw new ArgumentException("Last name is empty.");
            }

            if (userInputData.FirstName.Length < MinNumberOfSymbols || userInputData.FirstName.Length > MaxNumberOfSymbols ||
                !Regex.IsMatch(userInputData.FirstName, AllowedCharacters))
            {
                throw new ArgumentException("First name's length is out of range or has invalid characters.");
            }

            if (userInputData.LastName.Length < MinNumberOfSymbols || userInputData.LastName.Length > MaxNumberOfSymbols ||
                !Regex.IsMatch(userInputData.LastName, AllowedCharacters))
            {
                throw new ArgumentException("Last name's length is out of range or has invalid characters.");
            }

            if (userInputData.DateOfBirth < MinDateOfBirth || userInputData.DateOfBirth >= DateTime.Now)
            {
                throw new ArgumentException("Invalid date of birth.");
            }

            if (userInputData.Sex != MaleSex && userInputData.Sex != FemaleSex)
            {
                throw new ArgumentException("Wrong sex.");
            }

            if (userInputData.NumberOfReviews < MinNumberOfReviewsCustom)
            {
                throw new ArgumentException("Number of reviews is too small.");
            }

            if (userInputData.Salary < MinValueOfSalaryCustom)
            {
                throw new ArgumentException("Salary is too small.");
            }
        }
    }
}

using System;
using static FileCabinetApp.Constants;

namespace FileCabinetApp
{
    /// <summary>Performs default validation on user input data.</summary>
    /// <seealso cref="IRecordValidator" />
    public sealed class DefaultValidator : IRecordValidator
    {
        /// <summary>Validates user input parameters using default validation rules.</summary>
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

            if (userInputData.FirstName.Length < MinNumberOfSymbols || userInputData.FirstName.Length > MaxNumberOfSymbols)
            {
                throw new ArgumentException("First name's length is out of range.");
            }

            if (userInputData.LastName.Length < MinNumberOfSymbols || userInputData.LastName.Length > MaxNumberOfSymbols)
            {
                throw new ArgumentException("Last name's length is out of range.");
            }

            if (userInputData.DateOfBirth >= DateTime.Now)
            {
                throw new ArgumentException("Invalid date of birth.");
            }

            if (userInputData.Sex != MaleSex && userInputData.Sex != FemaleSex)
            {
                throw new ArgumentException("Wrong sex.");
            }

            if (userInputData.NumberOfReviews < MinNumberOfReviews)
            {
                throw new ArgumentException("Number of reviews cannot be less than zero.");
            }

            if (userInputData.Salary < MinValueOfSalary)
            {
                throw new ArgumentException("Salary cannot be less than zero.");
            }
        }
    }
}

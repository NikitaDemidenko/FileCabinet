using System;
using static FileCabinetApp.Constants;

namespace FileCabinetApp
{
    /// <summary>Performs default validation on user input data.</summary>
    /// <seealso cref="IRecordValidator" />
    public sealed class DefaultValidator : IRecordValidator
    {
        /// <summary>Validates user input parameters using default validation rules.</summary>
        /// <param name="unverifiedData">Raw data.</param>
        /// <exception cref="ArgumentNullException">Thrown when <em>userInput</em> is <em>null</em>.</exception>
        /// <exception cref="ArgumentException">Thrown when one of the user input parameters is invalid.</exception>
        public void ValidateParameters(UnverifiedData unverifiedData)
        {
            if (unverifiedData == null)
            {
                throw new ArgumentNullException(nameof(unverifiedData));
            }

            if (string.IsNullOrWhiteSpace(unverifiedData.FirstName))
            {
                throw new ArgumentException("First name is empty.");
            }

            if (string.IsNullOrWhiteSpace(unverifiedData.LastName))
            {
                throw new ArgumentException("Last name is empty.");
            }

            if (unverifiedData.FirstName.Length < MinNumberOfSymbols || unverifiedData.FirstName.Length > MaxNumberOfSymbols)
            {
                throw new ArgumentException("First name's length is out of range.");
            }

            if (unverifiedData.LastName.Length < MinNumberOfSymbols || unverifiedData.LastName.Length > MaxNumberOfSymbols)
            {
                throw new ArgumentException("Last name's length is out of range.");
            }

            if (unverifiedData.DateOfBirth >= DateTime.Now)
            {
                throw new ArgumentException("Invalid date of birth.");
            }

            if (unverifiedData.Sex != MaleSex && unverifiedData.Sex != FemaleSex)
            {
                throw new ArgumentException("Wrong sex.");
            }

            if (unverifiedData.NumberOfReviews < MinNumberOfReviews)
            {
                throw new ArgumentException("Number of reviews cannot be less than zero.");
            }

            if (unverifiedData.Salary < MinValueOfSalary)
            {
                throw new ArgumentException("Salary cannot be less than zero.");
            }
        }
    }
}

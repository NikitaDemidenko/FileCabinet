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
        /// <param name="unverifiedData">Raw data.</param>
        /// <exception cref="ArgumentNullException">Thrown when <em>userInput</em> is <em>null</em>.</exception>
        /// <exception cref="ArgumentException">Thrown when one of the user input parameters is invalid.</exception>
        public void ValidateParameters(UnverifiedData unverifiedData)
        {
            if (unverifiedData == null)
            {
                throw new ArgumentNullException(nameof(unverifiedData));
            }

            this.ValidateFirstName(unverifiedData.FirstName);
            this.ValidateLastName(unverifiedData.LastName);
            this.ValidateDateOfBirth(unverifiedData.DateOfBirth);
            this.ValidateSex(unverifiedData.Sex);
            this.ValidateNumberOfReviews(unverifiedData.NumberOfReviews);
            this.ValidateSalary(unverifiedData.Salary);
        }

        private void ValidateFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("First name is empty.");
            }

            if (firstName.Length < MinNumberOfSymbols || firstName.Length > MaxNumberOfSymbols ||
                !Regex.IsMatch(firstName, AllowedCharacters))
            {
                throw new ArgumentException("First name's length is out of range or has invalid characters.");
            }
        }

        private void ValidateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("Last name is empty.");
            }

            if (lastName.Length < MinNumberOfSymbols || lastName.Length > MaxNumberOfSymbols ||
                !Regex.IsMatch(lastName, AllowedCharacters))
            {
                throw new ArgumentException("Last name's length is out of range or has invalid characters.");
            }
        }

        private void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth < MinDateOfBirth || dateOfBirth >= DateTime.Now)
            {
                throw new ArgumentException("Invalid date of birth.");
            }
        }

        private void ValidateSex(char sex)
        {
            if (sex != MaleSex && sex != FemaleSex)
            {
                throw new ArgumentException("Wrong sex.");
            }
        }

        private void ValidateNumberOfReviews(short numberOfReviews)
        {
            if (numberOfReviews < MinNumberOfReviewsCustom)
            {
                throw new ArgumentException("Number of reviews is too small.");
            }
        }

        private void ValidateSalary(decimal salary)
        {
            if (salary < MinValueOfSalaryCustom)
            {
                throw new ArgumentException("Salary is too small.");
            }
        }
    }
}

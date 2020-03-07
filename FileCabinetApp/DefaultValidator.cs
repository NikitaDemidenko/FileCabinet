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

            if (firstName.Length < MinNumberOfSymbols || firstName.Length > MaxNumberOfSymbols)
            {
                throw new ArgumentException("First name's length is out of range.");
            }
        }

        private void ValidateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("Last name is empty.");
            }

            if (lastName.Length < MinNumberOfSymbols || lastName.Length > MaxNumberOfSymbols)
            {
                throw new ArgumentException("Last name's length is out of range.");
            }
        }

        private void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth >= DateTime.Now)
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
            if (numberOfReviews < MinNumberOfReviews)
            {
                throw new ArgumentException("Number of reviews cannot be less than zero.");
            }
        }

        private void ValidateSalary(decimal salary)
        {
            if (salary < MinValueOfSalary)
            {
                throw new ArgumentException("Salary cannot be less than zero.");
            }
        }
    }
}

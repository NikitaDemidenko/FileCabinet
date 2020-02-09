using System;

namespace FileCabinetApp
{
    /// <summary>Provides methods for user data input.</summary>
    public sealed class UserInputData
    {
        /// <summary>Initializes a new instance of the <see cref="UserInputData"/> class.</summary>
        /// <param name="firstName">First name user input.</param>
        /// <param name="lastName">Last name user input.</param>
        /// <param name="dateOfBirth">Date of birth user input.</param>
        /// <param name="sex">Sex user input.</param>
        /// <param name="numberOfReviews">Number of reviews user input.</param>
        /// <param name="salary">Salary user input.</param>
        public UserInputData(string firstName, string lastName, DateTime dateOfBirth, char sex, short numberOfReviews, decimal salary)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Sex = sex;
            this.NumberOfReviews = numberOfReviews;
            this.Salary = salary;
        }

        /// <summary>Gets the first name.</summary>
        /// <value>First name.</value>
        public string FirstName { get; }

        /// <summary>Gets the last name.</summary>
        /// <value>Last name.</value>
        public string LastName { get; }

        /// <summary>Gets the date of birth.</summary>
        /// <value>Date of birth.</value>
        public DateTime DateOfBirth { get; }

        /// <summary>Gets the sex.</summary>
        /// <value>Sex.</value>
        public char Sex { get; }

        /// <summary>Gets the number of reviews.</summary>
        /// <value>Number of reviews.</value>
        public short NumberOfReviews { get; }

        /// <summary>Gets the salary.</summary>
        /// <value>Salary.</value>
        public decimal Salary { get; }
    }
}

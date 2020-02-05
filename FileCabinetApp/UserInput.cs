using System;
using System.Globalization;
using System.Text.RegularExpressions;
using static FileCabinetApp.Constants;

namespace FileCabinetApp
{
    /// <summary>Provides methods for user data input.</summary>
    public sealed class UserInput
    {
        private readonly string firstName;
        private readonly string lastName;
        private readonly DateTime dateOfBirth;
        private readonly char sex;
        private readonly short numberOfReviews;
        private readonly decimal salary;

        /// <summary>Initializes a new instance of the <see cref="UserInput"/> class.</summary>
        public UserInput()
        {
            do
            {
                Console.Write("First name: ");
                this.firstName = Console.ReadLine();
                if (!Regex.IsMatch(this.firstName, AllowedCharacters) || this.firstName.Length < MinNumberOfSymbols || this.firstName.Length > MaxNumberOfSymbols)
                {
                    Console.WriteLine("Invalid first name. Try again!");
                }
                else
                {
                    break;
                }
            }
            while (IsInvalidInput);

            do
            {
                Console.Write("Last name: ");
                this.lastName = Console.ReadLine();
                if (!Regex.IsMatch(this.lastName, AllowedCharacters) || this.lastName.Length < MinNumberOfSymbols || this.lastName.Length > MaxNumberOfSymbols)
                {
                    Console.WriteLine("Invalid last name. Try again!");
                }
                else
                {
                    break;
                }
            }
            while (IsInvalidInput);

            do
            {
                Console.Write("Date of birth (MM/dd/yyyy): ");
                string input = Console.ReadLine();
                if (DateTime.TryParseExact(input, InputDateFormat, null, DateTimeStyles.None, out this.dateOfBirth) &&
                    this.dateOfBirth >= MinDateOfBirth && this.dateOfBirth < DateTime.Now)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid date. Try again!");
                }
            }
            while (IsInvalidInput);

            do
            {
                Console.Write("Sex: ");
                this.sex = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (this.sex == MaleSex || this.sex == FemaleSex)
                {
                    break;
                }

                Console.WriteLine("Invalid character. Try again!");
            }
            while (IsInvalidInput);

            do
            {
                Console.Write("Number of reviews: ");
                if (short.TryParse(Console.ReadLine(), out this.numberOfReviews) && this.numberOfReviews >= MinNumberOfReviews)
                {
                    break;
                }

                Console.WriteLine("Invalid characters!");
            }
            while (IsInvalidInput);

            do
            {
                Console.Write("Salary: ");
                if (decimal.TryParse(Console.ReadLine(), NumberStyles.Float, Culture, out this.salary) && this.salary >= MinValueOfSalary)
                {
                    break;
                }

                Console.WriteLine("Invalid characters!");
            }
            while (IsInvalidInput);
        }

        /// <summary>Gets the first name.</summary>
        /// <value>First name.</value>
        public string FirstName => this.firstName;

        /// <summary>Gets the last name.</summary>
        /// <value>Last name.</value>
        public string LastName => this.lastName;

        /// <summary>Gets the date of birth.</summary>
        /// <value>Date of birth.</value>
        public DateTime DateOfBirth => this.dateOfBirth;

        /// <summary>Gets the sex.</summary>
        /// <value>Sex.</value>
        public char Sex => this.sex;

        /// <summary>Gets the number of reviews.</summary>
        /// <value>Number of reviews.</value>
        public short NumberOfReviews => this.numberOfReviews;

        /// <summary>Gets the salary.</summary>
        /// <value>Salary.</value>
        public decimal Salary => this.salary;
    }
}

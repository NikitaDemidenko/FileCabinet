using System;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>Provides constatnts for work with FileCabinetApp.</summary>
    public static class Constants
    {
        /// <summary>Indicates whether the user input is invalid.</summary>
        public const bool IsInvalidInput = true;

        /// <summary>Male sex.</summary>
        public const char MaleSex = 'M';

        /// <summary>Female sex.</summary>
        public const char FemaleSex = 'F';

        /// <summary>Minimum value of salary.</summary>
        public const decimal MinValueOfSalary = 0M;

        /// <summary>Minimum number of symbols.</summary>
        public const int MinNumberOfSymbols = 2;

        /// <summary>Maximum number of symbols.</summary>
        public const int MaxNumberOfSymbols = 60;

        /// <summary>Minimum value of identifier.</summary>
        public const int MinValueOfId = 1;

        /// <summary>Number of parameters.</summary>
        public const int NumberOfParameters = 2;

        /// <summary>Index of the first parameter.</summary>
        public const int FirstParameterIndex = 0;

        /// <summary>Index of the second parameter.</summary>
        public const int SecondParameterIndex = 1;

        /// <summary>Minimum number of reviews.</summary>
        public const short MinNumberOfReviews = 0;

        /// <summary>Input date format.</summary>
        public const string InputDateFormat = "MM/dd/yyyy";

        /// <summary>Output date format.</summary>
        public const string OutputDateFormat = "yyyy-MMM-dd";

        /// <summary>Allowed characters for user input.</summary>
        public const string AllowedCharacters = @"^[a-zA-Z-]+$";

        /// <summary>First name property name.</summary>
        public const string FirstNamePropertyName = "FirstName";

        /// <summary>Last name property name.</summary>
        public const string LastNamePropertyName = "LastName";

        /// <summary>Date of birth property name.</summary>
        public const string DateOfBirthPropertyName = "DateOfBirth";

        /// <summary>Currency format.</summary>
        public const string CurrencyFormat = "C";

        /// <summary>Space symbol.</summary>
        public const char SpaceSymbol = ' ';

        /// <summary>Quote symbol.</summary>
        public const char QuoteSymbol = '"';

        /// <summary>Minimum date of birth.</summary>
        public static readonly DateTime MinDateOfBirth = new DateTime(1950, 01, 01);

        /// <summary>Culture.</summary>
        public static readonly CultureInfo Culture = new CultureInfo("en-US");
    }
}

using System;
using System.Globalization;

namespace FileCabinetApp
{
    public static class Constants
    {
        public const bool IsInvalidInput = true;

        public const char MaleSex = 'M';

        public const char FemaleSex = 'F';

        public const decimal MinValueOfSalary = 0M;

        public const int MinNumberOfSymbols = 2;

        public const int MaxNumberOfSymbols = 60;

        public const int MinValueOfId = 1;

        public const int NumberOfParameters = 2;

        public const int FirstParameterIndex = 0;

        public const int SecondParameterIndex = 1;

        public const short MinNumberOfReviews = 0;

        public const string InputDateFormat = "MM/dd/yyyy";

        public const string OutputDateFormat = "yyyy-MMM-dd";

        public const string AllowedCharacters = @"^[a-zA-Z-]+$";

        public const string FirstNamePropertyName = "FirstName";

        public const string LastNamePropertyName = "LastName";

        public const string DateOfBirthPropertyName = "DateOfBirth";

        public const string CurrencyFormat = "C";

        public const char SpaceSymbol = ' ';

        public const char QuoteSymbol = '"';

        public static readonly DateTime MinDateOfBirth = new DateTime(1950, 01, 01);

        public static readonly CultureInfo Culture = new CultureInfo("en-US");
    }
}

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

        /// <summary>Minimum value of salary (custom).</summary>
        public const decimal MinValueOfSalaryCustom = 200M;

        /// <summary>Minimum number of symbols.</summary>
        public const int MinNumberOfSymbols = 2;

        /// <summary>Maximum number of symbols.</summary>
        public const int MaxNumberOfSymbols = 60;

        /// <summary>Minimum value of identifier.</summary>
        public const int MinValueOfId = 1;

        /// <summary>Number of parameters.</summary>
        public const int NumberOfParameters = 2;

        /// <summary>Index of the first element of the collection.</summary>
        public const int FirstElementIndex = 0;

        /// <summary>Index of the second element of the collection.</summary>
        public const int SecondElementIndex = 1;

        /// <summary>Minimum number of reviews.</summary>
        public const short MinNumberOfReviews = 0;

        /// <summary>Minimum number of reviews (custom).</summary>
        public const short MinNumberOfReviewsCustom = 50;

        /// <summary>The record's lenght in bytes.</summary>
        public const int RecordLenghtInBytes = 278;

        /// <summary>First name offset.</summary>
        public const int FirstNameOffset = 6;

        /// <summary>Last name offset.</summary>
        public const int LastNameOffset = 126;

        /// <summary>Date of birth offset.</summary>
        public const int DateOfBirthOffset = 246;

        /// <summary>Identifier offset.</summary>
        public const int IdOffset = 2;

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

        /// <summary>Validation rules full property name.</summary>
        public const string ValidationRulesFullPropertyName = "--VALIDATION-RULES";

        /// <summary>Validation rules shortcut property name.</summary>
        public const string ValidationRulesShortcutPropertyName = "-V";

        /// <summary>Storage full property name.</summary>
        public const string StorageFullPropertyName = "--STORAGE";

        /// <summary>Storage shortcut property name.</summary>
        public const string StorageShortcutPropertyName = "-S";

        /// <summary>Default validation rules name.</summary>
        public const string DefaultValidationRulesName = "DEFAULT";

        /// <summary>Custom validation rules name.</summary>
        public const string CustomValidationRulesName = "CUSTOM";

        /// <summary>Memory storage argument name.</summary>
        public const string MemoryStorageName = "MEMORY";

        /// <summary>File storage argument name.</summary>
        public const string FileStorageName = "FILE";

        /// <summary>The database file name.</summary>
        public const string DbFileName = "cabinet-records.db";

        /// <summary>Currency format.</summary>
        public const string CurrencyFormat = "C";

        /// <summary>CSV file extension.</summary>
        public const string CsvFileExtension = "csv";

        /// <summary>XML file extension.</summary>
        public const string XmlFileExtension = "xml";

        /// <summary>Positive user answer.</summary>
        public const string PositiveUserAnswer = "Y";

        /// <summary>Space symbol.</summary>
        public const char SpaceSymbol = ' ';

        /// <summary>Quote symbol.</summary>
        public const char QuoteSymbol = '"';

        /// <summary>An equal sign symbol.</summary>
        public const char EqualSignSymbol = '=';

        /// <summary>Null character.</summary>
        public const char NullCharacter = '\0';

        /// <summary>Minimum date of birth.</summary>
        public static readonly DateTime MinDateOfBirth = new DateTime(1950, 01, 01);

        /// <summary>Culture.</summary>
        public static readonly CultureInfo Culture = new CultureInfo("en-US");
    }
}

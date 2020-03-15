using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FileCabinetApp.ConstantsAndValidationRulesSettings
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

        /// <summary>Minimum value of identifier.</summary>
        public const int MinValueOfId = 1;

        /// <summary>Number of parameters.</summary>
        public const int NumberOfParameters = 2;

        /// <summary>Index of the first element of the collection.</summary>
        public const int FirstElementIndex = 0;

        /// <summary>Index of the second element of the collection.</summary>
        public const int SecondElementIndex = 1;

        /// <summary>First name offset.</summary>
        public const int FirstNameOffset = 6;

        /// <summary>Identifier offset.</summary>
        public const int IdOffset = 2;

        /// <summary>The deleted bit bit mask.</summary>
        public const short DeletedBitMask = 4;

        /// <summary>Begin of the file.</summary>
        public const long BeginOfFile = 0;

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

        /// <summary>Create command.</summary>
        public const string CreateCommand = "create";

        /// <summary>Edit command.</summary>
        public const string EditCommand = "edit";

        /// <summary>Exit command.</summary>
        public const string ExitCommand = "exit";

        /// <summary>Export command.</summary>
        public const string ExportCommand = "export";

        /// <summary>Find command.</summary>
        public const string FindCommand = "find";

        /// <summary>Help command.</summary>
        public const string HelpCommand = "help";

        /// <summary>Import command.</summary>
        public const string ImportCommand = "import";

        /// <summary>List command.</summary>
        public const string ListCommand = "list";

        /// <summary>Purge command.</summary>
        public const string PurgeCommand = "purge";

        /// <summary>Remove command.</summary>
        public const string RemoveCommand = "remove";

        /// <summary>Stat command.</summary>
        public const string StatCommand = "stat";

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

        /// <summary>The stopwatch property name.</summary>
        public const string StopwatchPropertyName = "--USE-STOPWATCH";

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

        /// <summary>CSV file separator.</summary>
        public const char CsvFileSeparator = ';';

        private static readonly Limits Limits = JsonSerializer.Deserialize<Limits>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "validation-rules.json")));

        /// <summary>Culture.</summary>
        public static readonly CultureInfo Culture = new CultureInfo("en-US");

        /// <summary>Minimum value of salary (default).</summary>
        public static readonly decimal MinValueOfSalary = Limits.Default.SalaryLimits.MinSalary;

        /// <summary>Minimum value of salary (custom).</summary>
        public static readonly decimal MinValueOfSalaryCustom = Limits.Custom.SalaryLimits.MinSalary;

        /// <summary>The minimum first name length.</summary>
        public static readonly int MinFirstNameLength = Limits.Default.FirstNameLimits.MinLength;

        /// <summary>The minimum first name length (custom).</summary>
        public static readonly int MinFirstNameLengthCustom = Limits.Custom.FirstNameLimits.MinLength;

        /// <summary>The maximum first name length.</summary>
        public static readonly int MaxFirstNameLength = Limits.Default.FirstNameLimits.MaxLength;

        /// <summary>The maximum first name length (custom).</summary>
        public static readonly int MaxFirstNameLengthCustom = Limits.Custom.FirstNameLimits.MaxLength;

        /// <summary>The minimum last name length.</summary>
        public static readonly int MinLastNameLength = Limits.Default.LastNameLimits.MinLength;

        /// <summary>The minimum last name length (custom).</summary>
        public static readonly int MinLastNameLengthCustom = Limits.Custom.LastNameLimits.MinLength;

        /// <summary>The maximum last name length.</summary>
        public static readonly int MaxLastNameLength = Limits.Default.LastNameLimits.MaxLength;

        /// <summary>The maximum last name length (custom).</summary>
        public static readonly int MaxLastNameLengthCustom = Limits.Custom.LastNameLimits.MaxLength;

        /// <summary>Minimum number of reviews.</summary>
        public static readonly short MinNumberOfReviews = Limits.Default.NumberOfReviewsLimits.MinNumber;

        /// <summary>Minimum number of reviews (custom).</summary>
        public static readonly short MinNumberOfReviewsCustom = Limits.Custom.NumberOfReviewsLimits.MinNumber;

        /// <summary>Minimum date of birth (custom).</summary>
        public static readonly DateTime MinDateOfBirthCustom = DateTime.Parse(Limits.Custom.DateOfBirthLimits.From, Culture);

        /// <summary>Minimum date of birth
        /// (default).</summary>
        public static readonly DateTime MinDateOfBirth = DateTime.Parse(Limits.Default.DateOfBirthLimits.From, Culture);

        /// <summary>Maximum date of birth
        /// (default).</summary>
        public static readonly DateTime MaxDateOfBirth = DateTime.Parse(Limits.Default.DateOfBirthLimits.To, Culture);

        /// <summary>Maximum date of birth (custom).</summary>
        public static readonly DateTime MaxDateOfBirthCustom = DateTime.Parse(Limits.Custom.DateOfBirthLimits.To, Culture);

        /// <summary>The record's lenght in bytes.</summary>
        public static readonly int RecordLenghtInBytes = (2 * sizeof(short)) + (2 * MaxFirstNameLength) + (2 * MaxLastNameLength) + (4 * sizeof(int)) + sizeof(char) + sizeof(decimal);

        /// <summary>Last name offset.</summary>
        public static readonly int LastNameOffset = FirstNameOffset + (2 * MaxFirstNameLength);

        /// <summary>Date of birth offset.</summary>
        public static readonly int DateOfBirthOffset = LastNameOffset + (2 * MaxLastNameLength);
    }
}

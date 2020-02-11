using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using static FileCabinetApp.Constants;

namespace FileCabinetApp
{
    /// <summary>Main class of the project.</summary>
    public static class Program
    {
        private const string DeveloperName = "Nikita Demidenko";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static bool isRunning = true;
        private static bool isCustomValidationRules = false;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("exit", Exit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "create", "creates new record", "The 'create' command creates new record." },
            new string[] { "edit", "edits a record by Id", "The 'edit' command edits a record by Id." },
            new string[] { "find", "finds records by selected property", "The 'find' command finds records by selected property" },
            new string[] { "list", "prints all records", "The 'list' command prints the records." },
            new string[] { "stat", "shows the number of records", "The 'stat' command shows the number of records." },
            new string[] { "export", "exports records to file", "The 'export' command exports records to file." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        private static IFileCabinetService fileCabinetService;

        private static Func<string, Tuple<bool, string, string>> stringConverter = input =>
        {
            return new Tuple<bool, string, string>(true, null, input);
        };

        private static Func<string, Tuple<bool, string, DateTime>> dateConverter = input =>
        {
            return DateTime.TryParseExact(input, InputDateFormat, null, DateTimeStyles.None, out DateTime dateOfBirth)
                ? new Tuple<bool, string, DateTime>(true, null, dateOfBirth)
                : new Tuple<bool, string, DateTime>(false, "Invalid date format/characters", dateOfBirth);
        };

        private static Func<string, Tuple<bool, string, char>> charConverter = input =>
        {
            return char.TryParse(input, out char sex)
                ? new Tuple<bool, string, char>(true, null, sex)
                : new Tuple<bool, string, char>(false, "Input length must be equal to one", sex);
        };

        private static Func<string, Tuple<bool, string, short>> numberOfReviewsConverter = input =>
        {
            return short.TryParse(input, out short numberOfReviews)
                ? new Tuple<bool, string, short>(true, null, numberOfReviews)
                : new Tuple<bool, string, short>(false, "Invalid characters", numberOfReviews);
        };

        private static Func<string, Tuple<bool, string, decimal>> salaryConverter = input =>
        {
            return decimal.TryParse(input, out decimal salary)
                ? new Tuple<bool, string, decimal>(true, null, salary)
                : new Tuple<bool, string, decimal>(false, "Invalid characters", salary);
        };

        private static Func<string, Tuple<bool, string>> firstNameValidator = firstName =>
        {
            return isCustomValidationRules
                ? firstName.Length < MinNumberOfSymbols || firstName.Length > MaxNumberOfSymbols ||
                !Regex.IsMatch(firstName, AllowedCharacters)
                    ? new Tuple<bool, string>(false, "First name's length is out of range or has invalid characters")
                    : new Tuple<bool, string>(true, null)
                : firstName.Length < MinNumberOfSymbols || firstName.Length > MaxNumberOfSymbols
                    ? new Tuple<bool, string>(false, "First name's length is out of range")
                    : new Tuple<bool, string>(true, null);
        };

        private static Func<string, Tuple<bool, string>> lastNameValidator = lastName =>
        {
            return isCustomValidationRules
                ? lastName.Length < MinNumberOfSymbols || lastName.Length > MaxNumberOfSymbols ||
                !Regex.IsMatch(lastName, AllowedCharacters)
                    ? new Tuple<bool, string>(false, "First name's length is out of range or has invalid characters")
                    : new Tuple<bool, string>(true, null)
                : lastName.Length < MinNumberOfSymbols || lastName.Length > MaxNumberOfSymbols
                    ? new Tuple<bool, string>(false, "First name's length is out of range")
                    : new Tuple<bool, string>(true, null);
        };

        private static Func<DateTime, Tuple<bool, string>> dateOfBirthValidator = dateOfBirth =>
        {
            return isCustomValidationRules
                ? dateOfBirth < MinDateOfBirth || dateOfBirth >= DateTime.Now
                    ? new Tuple<bool, string>(false, "Date of birth is greater than the current date or less than 1950-Jan-01")
                    : new Tuple<bool, string>(true, null)
                : dateOfBirth >= DateTime.Now
                    ? new Tuple<bool, string>(false, "Date of birth is greater than the current date")
                    : new Tuple<bool, string>(true, null);
        };

        private static Func<char, Tuple<bool, string>> sexValidator = sex =>
        {
            return sex != MaleSex && sex != FemaleSex ? new Tuple<bool, string>(false, "Wrong sex") : new Tuple<bool, string>(true, null);
        };

        private static Func<short, Tuple<bool, string>> numberOfReviewsValidator = numberOfReviews =>
        {
            return isCustomValidationRules
                ? numberOfReviews < MinNumberOfReviewsCustom
                    ? new Tuple<bool, string>(false, "Number of reviews is too small. It must be greater than 50")
                    : new Tuple<bool, string>(true, null)
                : numberOfReviews < MinNumberOfReviews
                    ? new Tuple<bool, string>(false, "Number of reviews cannot be less than zero")
                    : new Tuple<bool, string>(true, null);
        };

        private static Func<decimal, Tuple<bool, string>> salaryValidator = salary =>
        {
            return isCustomValidationRules
                ? salary < MinValueOfSalaryCustom
                    ? new Tuple<bool, string>(false, "Salary is too small. It must be greater than 200")
                    : new Tuple<bool, string>(true, null)
                : salary < MinValueOfSalary
                    ? new Tuple<bool, string>(false, "Salary cannot be less than zero")
                    : new Tuple<bool, string>(true, null);
        };

        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            if (!TryParseCommandLine(args))
            {
                Console.WriteLine("Invalid parameters.");
                Console.WriteLine("Using default validation rules.");
                fileCabinetService = new FileCabinetService(new DefaultValidator());
            }

            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            Console.Write("First name: ");
            var firstName = ReadInput(stringConverter, firstNameValidator);

            Console.Write("Last name: ");
            var lastName = ReadInput(stringConverter, lastNameValidator);

            Console.Write("Date of birth: ");
            var dateOfBirth = ReadInput(dateConverter, dateOfBirthValidator);

            Console.Write("Sex: ");
            var sex = ReadInput(charConverter, sexValidator);

            Console.Write("Number of reviews: ");
            var numberOfReviews = ReadInput(numberOfReviewsConverter, numberOfReviewsValidator);

            Console.Write("Salary: ");
            var salary = ReadInput(salaryConverter, salaryValidator);

            var userInputData = new UserInputData(firstName, lastName, dateOfBirth, sex, numberOfReviews, salary);
            fileCabinetService.CreateRecord(userInputData);
            Console.WriteLine($"Record #{fileCabinetService.GetStat()} is created.");
        }

        private static void List(string parameters)
        {
            if (fileCabinetService.GetStat() == 0)
            {
                Console.WriteLine("There are no records.");
                return;
            }

            var records = fileCabinetService.GetRecords();
            foreach (var record in records)
            {
                Console.WriteLine(record);
            }
        }

        private static void Edit(string parameters)
        {
            if (!int.TryParse(parameters, out int id))
            {
                Console.WriteLine("Invalid characters.");
                Console.WriteLine();
                return;
            }

            if (id < MinValueOfId || id > fileCabinetService.GetStat())
            {
                Console.WriteLine($"#{id} record is not found.");
                Console.WriteLine();
                return;
            }

            Console.Write("First name: ");
            var firstName = ReadInput(stringConverter, firstNameValidator);

            Console.Write("Last name: ");
            var lastName = ReadInput(stringConverter, lastNameValidator);

            Console.Write("Date of birth: ");
            var dateOfBirth = ReadInput(dateConverter, dateOfBirthValidator);

            Console.Write("Sex: ");
            var sex = ReadInput(charConverter, sexValidator);

            Console.Write("Number of reviews: ");
            var numberOfReviews = ReadInput(numberOfReviewsConverter, numberOfReviewsValidator);

            Console.Write("Salary: ");
            var salary = ReadInput(salaryConverter, salaryValidator);

            var userInputData = new UserInputData(firstName, lastName, dateOfBirth, sex, numberOfReviews, salary);
            fileCabinetService.EditRecord(id, userInputData);
            Console.WriteLine($"Record #{id} is updated.");
        }

        private static void Find(string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                Console.WriteLine("Enter search property.");
                return;
            }

            string[] splittedParameters = parameters.Split(SpaceSymbol, NumberOfParameters, StringSplitOptions.RemoveEmptyEntries);
            string propertyName;
            string propertyValue;
            try
            {
                propertyName = splittedParameters[FirstElementIndex];
                propertyValue = splittedParameters[SecondElementIndex].Trim(QuoteSymbol);
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Invalid number of parameters.");
                return;
            }

            if (propertyName.Equals(FirstNamePropertyName, StringComparison.InvariantCultureIgnoreCase))
            {
                var searchResult = fileCabinetService.FindByFirstName(propertyValue);
                if (searchResult != null)
                {
                    foreach (var record in searchResult)
                    {
                        Console.WriteLine(record);
                    }
                }
                else
                {
                    Console.WriteLine($"Records with first name \"{propertyValue}\" are not found.");
                }
            }
            else if (propertyName.Equals(LastNamePropertyName, StringComparison.InvariantCultureIgnoreCase))
            {
                var searchResult = fileCabinetService.FindByLastName(propertyValue);
                if (searchResult != null)
                {
                    foreach (var record in searchResult)
                    {
                        Console.WriteLine(record);
                    }
                }
                else
                {
                    Console.WriteLine($"Records with last name \"{propertyValue}\" are not found.");
                }
            }
            else if (propertyName.Equals(DateOfBirthPropertyName, StringComparison.InvariantCultureIgnoreCase))
            {
                if (DateTime.TryParse(propertyValue, out DateTime dateOfBirth))
                {
                    var searchResult = fileCabinetService.FindByDateOfBirth(dateOfBirth);
                    if (searchResult != null)
                    {
                        foreach (var record in searchResult)
                        {
                            Console.WriteLine(record);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Records with date of birth \"{dateOfBirth.ToString(OutputDateFormat, CultureInfo.InvariantCulture)}\" are not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid date.");
                }
            }
            else
            {
                Console.WriteLine("Invalid property.");
            }
        }

        private static void Export(string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                Console.WriteLine("Enter export parameters.");
                return;
            }

            var splittedParameters = parameters.Split(SpaceSymbol, NumberOfParameters, StringSplitOptions.RemoveEmptyEntries);
            string typeOfFile;
            string filePath;
            try
            {
                typeOfFile = splittedParameters[FirstElementIndex];
                filePath = splittedParameters[SecondElementIndex].Trim(QuoteSymbol);
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Invalid number of parameters.");
                return;
            }

            if (typeOfFile.Equals(CsvFileExtension, StringComparison.InvariantCultureIgnoreCase) ||
                typeOfFile.Equals(XmlFileExtension, StringComparison.InvariantCultureIgnoreCase))
            {
                if (File.Exists(filePath))
                {
                    Console.Write($"File is exist - rewrite {filePath}? [Y/n]: ");
                    string answer = Console.ReadLine();
                    Console.WriteLine();
                    if (!answer.Equals(PositiveUserAnswer, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine("Export has been canceled.");
                        return;
                    }
                }

                bool append = false;
                try
                {
                    using var streamWriter = new StreamWriter(filePath, append, Encoding.UTF8);
                    var snapshot = fileCabinetService.MakeSnapshot();
                    if (typeOfFile.Equals(CsvFileExtension, StringComparison.InvariantCultureIgnoreCase))
                    {
                        snapshot.SaveToCsv(streamWriter);
                    }
                    else
                    {
                        snapshot.SaveToXml(streamWriter);
                    }

                    Console.WriteLine("Export completed.");
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
            else
            {
                Console.WriteLine("Wrong type of file parameter.");
            }
        }

        private static bool TryParseCommandLine(string[] args)
        {
            bool isValidInput = true;
            if (args == null)
            {
                return !isValidInput;
            }

            switch (args.Length)
            {
                case 0:
                    Console.WriteLine("Using default validation rules.");
                    fileCabinetService = new FileCabinetService(new DefaultValidator());
                    return isValidInput;
                case 1:
                    var splittedParameters = args[FirstElementIndex].Split(EqualSignSymbol, NumberOfParameters, StringSplitOptions.RemoveEmptyEntries);
                    string parameterName;
                    string parameterValue;
                    try
                    {
                        parameterName = splittedParameters[FirstElementIndex];
                        parameterValue = splittedParameters[SecondElementIndex];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return !isValidInput;
                    }

                    if (parameterName.Equals(ValidationRulesFullPropertyName, StringComparison.InvariantCultureIgnoreCase) &&
                        parameterValue.Equals(DefaultValidationRulesName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine("Using default validation rules.");
                        fileCabinetService = new FileCabinetService(new DefaultValidator());
                        return isValidInput;
                    }
                    else if (parameterName.Equals(ValidationRulesFullPropertyName, StringComparison.InvariantCultureIgnoreCase) &&
                         parameterValue.Equals(CustomValidationRulesName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine("Using custom validation rules.");
                        fileCabinetService = new FileCabinetService(new CustomValidator());
                        isCustomValidationRules = true;
                        return isValidInput;
                    }
                    else
                    {
                        return !isValidInput;
                    }

                case 2:
                    if (args[FirstElementIndex].Equals(ValidationRulesShortcutPropertyName, StringComparison.InvariantCultureIgnoreCase) &&
                    args[SecondElementIndex].Equals(DefaultValidationRulesName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine("Using default validation rules.");
                        fileCabinetService = new FileCabinetService(new DefaultValidator());
                        return isValidInput;
                    }
                    else if (args[FirstElementIndex].Equals(ValidationRulesShortcutPropertyName, StringComparison.InvariantCultureIgnoreCase) &&
                        args[SecondElementIndex].Equals(CustomValidationRulesName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine("Using custom validation rules.");
                        fileCabinetService = new FileCabinetService(new CustomValidator());
                        isCustomValidationRules = true;
                        return isValidInput;
                    }
                    else
                    {
                        return !isValidInput;
                    }

                default:
                    return !isValidInput;
            }
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (IsInvalidInput);
        }
    }
}

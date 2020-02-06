using System;
using System.Globalization;
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

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("stat", Stat),
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
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        private static FileCabinetService fileCabinetService;

        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            if (!TryParseCommandLine(args))
            {
                Console.WriteLine("Invalid parameters.");
                Console.WriteLine("Using default validation rules.");
                fileCabinetService = new FileCabinetDefaultService();
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
            GetUserInput(out string firstName, out string lastName, out DateTime dateOfBirth, out char sex, out short numberOfReviews, out decimal salary);
            var userInput = new UserInputData(firstName, lastName, dateOfBirth, sex, numberOfReviews, salary);
            try
            {
                fileCabinetService.CreateRecord(userInput);
                Console.WriteLine($"Record #{fileCabinetService.GetStat()} is created.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Record has not been created.");
                return;
            }
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
                return;
            }

            if (id < MinValueOfId || id > fileCabinetService.GetStat())
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }

            GetUserInput(out string firstName, out string lastName, out DateTime dateOfBirth, out char sex, out short numberOfReviews, out decimal salary);
            var userInput = new UserInputData(firstName, lastName, dateOfBirth, sex, numberOfReviews, salary);
            try
            {
                fileCabinetService.EditRecord(id, userInput);
                Console.WriteLine($"Record #{id} is updated.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Record has not been updated.");
                return;
            }
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
                propertyName = splittedParameters[FirstParameterIndex];
                propertyValue = splittedParameters[SecondParameterIndex].Trim(QuoteSymbol);
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Invalid number of parameters.");
                return;
            }

            if (propertyName.Equals(FirstNamePropertyName, StringComparison.InvariantCultureIgnoreCase))
            {
                var searchResult = fileCabinetService.FindByFirstName(propertyValue);
                if (searchResult.Length != 0)
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
                if (searchResult.Length != 0)
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
                    if (searchResult.Length != 0)
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

        private static void GetUserInput(
            out string firstName,
            out string lastName,
            out DateTime dateOfBirth,
            out char sex,
            out short numberOfReviews,
            out decimal salary)
        {
            Console.Write("First name: ");
            firstName = Console.ReadLine();
            Console.Write("Last name: ");
            lastName = Console.ReadLine();
            do
            {
                Console.Write("Date of birth (MM/dd/yyyy): ");
                if (DateTime.TryParseExact(Console.ReadLine(), InputDateFormat, null, DateTimeStyles.None, out dateOfBirth))
                {
                    break;
                }

                Console.WriteLine("Invalid date. Try again!");
            }
            while (IsInvalidInput);

            Console.Write("Sex: ");
            sex = Console.ReadKey().KeyChar;
            Console.WriteLine();
            do
            {
                Console.Write("Number of reviews: ");
                if (short.TryParse(Console.ReadLine(), out numberOfReviews))
                {
                    break;
                }

                Console.WriteLine("Invalid characters!");
            }
            while (IsInvalidInput);

            do
            {
                Console.Write("Salary: ");
                if (decimal.TryParse(Console.ReadLine(), NumberStyles.Float, Culture, out salary))
                {
                    break;
                }

                Console.WriteLine("Invalid characters!");
            }
            while (IsInvalidInput);
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
                    fileCabinetService = new FileCabinetDefaultService();
                    return isValidInput;
                case 1:
                    var splittedParameters = args[FirstParameterIndex].Split(EqualSignSymbol, NumberOfParameters, StringSplitOptions.RemoveEmptyEntries);
                    string parameterName;
                    string parameterValue;
                    try
                    {
                        parameterName = splittedParameters[FirstParameterIndex];
                        parameterValue = splittedParameters[SecondParameterIndex];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return !isValidInput;
                    }

                    if (parameterName.Equals(ValidationRulesFullPropertyName, StringComparison.InvariantCultureIgnoreCase) &&
                        parameterValue.Equals(DefaultValidationRulesName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine("Using default validation rules.");
                        fileCabinetService = new FileCabinetDefaultService();
                        return isValidInput;
                    }
                    else if (parameterName.Equals(ValidationRulesFullPropertyName, StringComparison.InvariantCultureIgnoreCase) &&
                         parameterValue.Equals(CustomValidationRulesName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine("Using custom validation rules.");
                        fileCabinetService = new FileCabinetCustomService();
                        return isValidInput;
                    }
                    else
                    {
                        return !isValidInput;
                    }

                case 2:
                    if (args[FirstParameterIndex].Equals(ValidationRulesShortcutPropertyName, StringComparison.InvariantCultureIgnoreCase) &&
                    args[SecondParameterIndex].Equals(DefaultValidationRulesName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine("Using default validation rules.");
                        fileCabinetService = new FileCabinetDefaultService();
                        return isValidInput;
                    }
                    else if (args[FirstParameterIndex].Equals(ValidationRulesShortcutPropertyName, StringComparison.InvariantCultureIgnoreCase) &&
                        args[SecondParameterIndex].Equals(CustomValidationRulesName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine("Using custom validation rules.");
                        fileCabinetService = new FileCabinetCustomService();
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
    }
}

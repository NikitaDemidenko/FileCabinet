using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Nikita Demidenko";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static readonly CultureInfo Culture = new CultureInfo("en-US");

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

        private static FileCabinetService fileCabinetService = new FileCabinetService();

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
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
            string firstName;
            do
            {
                Console.Write("First name: ");
                firstName = Console.ReadLine();
                if (!Regex.IsMatch(firstName, @"^[a-zA-Z-]+$") || firstName.Length < 2 || firstName.Length > 60)
                {
                    Console.WriteLine("Invalid first name. Try again!");
                }
                else
                {
                    break;
                }
            }
            while (true);

            string lastName;
            do
            {
                Console.Write("Last name: ");
                lastName = Console.ReadLine();
                if (!Regex.IsMatch(lastName, @"^[a-zA-Z-]+$") || lastName.Length < 2 || lastName.Length > 60)
                {
                    Console.WriteLine("Invalid last name. Try again!");
                }
                else
                {
                    break;
                }
            }
            while (true);

            DateTime dateOfBirth;
            string input;
            do
            {
                Console.Write("Date of birth (MM/dd/yyyy): ");
                input = Console.ReadLine();
                if (DateTime.TryParseExact(input, "MM/dd/yyyy", null, DateTimeStyles.None, out dateOfBirth) &&
                    dateOfBirth >= new DateTime(1950, 01, 01) && dateOfBirth < DateTime.Now)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid date. Try again!");
                }
            }
            while (true);

            char sex;
            do
            {
                Console.Write("Sex: ");
                sex = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (sex == 'M' || sex == 'F')
                {
                    break;
                }

                Console.WriteLine("Invalid character. Try again!");
            }
            while (true);

            short numberOfReviews;
            do
            {
                Console.Write("Number of reviews: ");
                if (short.TryParse(Console.ReadLine(), out numberOfReviews) && numberOfReviews >= 0)
                {
                    break;
                }

                Console.WriteLine("Invalid characters!");
            }
            while (true);

            decimal salary;
            do
            {
                Console.Write("Salary: ");
                if (decimal.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.DefaultThreadCurrentCulture, out salary) && salary >= 0)
                {
                    break;
                }

                Console.WriteLine("Invalid characters!");
            }
            while (true);

            fileCabinetService.CreateRecord(firstName, lastName, dateOfBirth, sex, numberOfReviews, salary);
            Console.WriteLine($"Record #{fileCabinetService.GetStat()} is created.");
        }

        private static void List(string parameters)
        {
            var records = fileCabinetService.GetRecords();
            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", Culture)}, " +
                    $"{record.Sex}, {record.NumberOfReviews}, {record.Salary.ToString("C", Culture)}");
            }
        }

        private static void Edit(string parameters)
        {
            if (!int.TryParse(parameters, out int id))
            {
                Console.WriteLine("Invalid characters.");
                return;
            }

            if (id < 1 || id > fileCabinetService.GetStat())
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }

            string firstName;
            do
            {
                Console.Write("First name: ");
                firstName = Console.ReadLine();
                if (!Regex.IsMatch(firstName, @"^[a-zA-Z-]+$") || firstName.Length < 2 || firstName.Length > 60)
                {
                    Console.WriteLine("Invalid first name. Try again!");
                }
                else
                {
                    break;
                }
            }
            while (true);

            string lastName;
            do
            {
                Console.Write("Last name: ");
                lastName = Console.ReadLine();
                if (!Regex.IsMatch(lastName, @"^[a-zA-Z-]+$") || lastName.Length < 2 || lastName.Length > 60)
                {
                    Console.WriteLine("Invalid last name. Try again!");
                }
                else
                {
                    break;
                }
            }
            while (true);

            DateTime dateOfBirth;
            string input;
            do
            {
                Console.Write("Date of birth (MM/dd/yyyy): ");
                input = Console.ReadLine();
                if (DateTime.TryParseExact(input, "MM/dd/yyyy", null, DateTimeStyles.None, out dateOfBirth) &&
                    dateOfBirth >= new DateTime(1950, 01, 01) && dateOfBirth < DateTime.Now)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid date. Try again!");
                }
            }
            while (true);

            char sex;
            do
            {
                Console.Write("Sex: ");
                sex = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (sex == 'M' || sex == 'F')
                {
                    break;
                }

                Console.WriteLine("Invalid character. Try again!");
            }
            while (true);

            short numberOfReviews;
            do
            {
                Console.Write("Number of reviews: ");
                if (short.TryParse(Console.ReadLine(), out numberOfReviews) && numberOfReviews >= 0)
                {
                    break;
                }

                Console.WriteLine("Invalid characters!");
            }
            while (true);

            decimal salary;
            do
            {
                Console.Write("Salary: ");
                if (decimal.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.DefaultThreadCurrentCulture, out salary) && salary >= 0)
                {
                    break;
                }

                Console.WriteLine("Invalid characters!");
            }
            while (true);

            try
            {
                fileCabinetService.EditRecord(id, firstName, lastName, dateOfBirth, sex, numberOfReviews, salary);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine($"Record #{id} is updated.");
        }

        private static void Find(string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                Console.WriteLine("Enter search property.");
                return;
            }

            string[] splittedParameters = parameters.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string propertyName;
            string propertyValue;
            try
            {
                propertyName = splittedParameters[0];
                propertyValue = splittedParameters[1].Trim('"');
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Invalid number of parameters.");
                return;
            }

            if (propertyName.Equals("firstname", StringComparison.InvariantCultureIgnoreCase))
            {
                var searchResult = fileCabinetService.FindByFirstName(propertyValue);
                if (searchResult.Length != 0)
                {
                    foreach (var record in searchResult)
                    {
                        Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", Culture)}, " +
                    $"{record.Sex}, {record.NumberOfReviews}, {record.Salary.ToString("C", Culture)}");
                    }
                }
                else
                {
                    Console.WriteLine($"Records with first name \"{propertyValue}\" are not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid property.");
            }
        }
    }
}

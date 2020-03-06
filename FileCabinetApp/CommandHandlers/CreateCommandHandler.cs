using System;
using System.Globalization;
using System.Text.RegularExpressions;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Create command handler.</summary>
    /// <seealso cref="CommandHandlerBase" />
    public class CreateCommandHandler : CommandHandlerBase
    {
        private Func<string, Tuple<bool, string, string>> stringConverter = input =>
        {
            return new Tuple<bool, string, string>(true, null, input);
        };

        private Func<string, Tuple<bool, string, DateTime>> dateConverter = input =>
        {
            return DateTime.TryParseExact(input, InputDateFormat, null, DateTimeStyles.None, out DateTime dateOfBirth)
                ? new Tuple<bool, string, DateTime>(true, null, dateOfBirth)
                : new Tuple<bool, string, DateTime>(false, "Invalid date format/characters", dateOfBirth);
        };

        private Func<string, Tuple<bool, string, char>> charConverter = input =>
        {
            return char.TryParse(input, out char sex)
                ? new Tuple<bool, string, char>(true, null, sex)
                : new Tuple<bool, string, char>(false, "Input length must be equal to one", sex);
        };

        private Func<string, Tuple<bool, string, short>> numberOfReviewsConverter = input =>
        {
            return short.TryParse(input, out short numberOfReviews)
                ? new Tuple<bool, string, short>(true, null, numberOfReviews)
                : new Tuple<bool, string, short>(false, "Invalid characters", numberOfReviews);
        };

        private Func<string, Tuple<bool, string, decimal>> salaryConverter = input =>
        {
            return decimal.TryParse(input, out decimal salary)
                ? new Tuple<bool, string, decimal>(true, null, salary)
                : new Tuple<bool, string, decimal>(false, "Invalid characters", salary);
        };

        private Func<string, Tuple<bool, string>> firstNameValidator = firstName =>
        {
            return Program.isCustomValidationRules
                ? firstName.Length < MinNumberOfSymbols || firstName.Length > MaxNumberOfSymbols ||
                !Regex.IsMatch(firstName, AllowedCharacters)
                    ? new Tuple<bool, string>(false, "First name's length is out of range or has invalid characters")
                    : new Tuple<bool, string>(true, null)
                : firstName.Length < MinNumberOfSymbols || firstName.Length > MaxNumberOfSymbols
                    ? new Tuple<bool, string>(false, "First name's length is out of range")
                    : new Tuple<bool, string>(true, null);
        };

        private Func<string, Tuple<bool, string>> lastNameValidator = lastName =>
        {
            return Program.isCustomValidationRules
                ? lastName.Length < MinNumberOfSymbols || lastName.Length > MaxNumberOfSymbols ||
                !Regex.IsMatch(lastName, AllowedCharacters)
                    ? new Tuple<bool, string>(false, "First name's length is out of range or has invalid characters")
                    : new Tuple<bool, string>(true, null)
                : lastName.Length < MinNumberOfSymbols || lastName.Length > MaxNumberOfSymbols
                    ? new Tuple<bool, string>(false, "First name's length is out of range")
                    : new Tuple<bool, string>(true, null);
        };

        private Func<DateTime, Tuple<bool, string>> dateOfBirthValidator = dateOfBirth =>
        {
            return Program.isCustomValidationRules
                ? dateOfBirth < MinDateOfBirth || dateOfBirth >= DateTime.Now
                    ? new Tuple<bool, string>(false, "Date of birth is greater than the current date or less than 1950-Jan-01")
                    : new Tuple<bool, string>(true, null)
                : dateOfBirth >= DateTime.Now
                    ? new Tuple<bool, string>(false, "Date of birth is greater than the current date")
                    : new Tuple<bool, string>(true, null);
        };

        private Func<char, Tuple<bool, string>> sexValidator = sex =>
        {
            return sex != MaleSex && sex != FemaleSex ? new Tuple<bool, string>(false, "Wrong sex") : new Tuple<bool, string>(true, null);
        };

        private Func<short, Tuple<bool, string>> numberOfReviewsValidator = numberOfReviews =>
        {
            return Program.isCustomValidationRules
                ? numberOfReviews < MinNumberOfReviewsCustom
                    ? new Tuple<bool, string>(false, "Number of reviews is too small. It must be greater than 50")
                    : new Tuple<bool, string>(true, null)
                : numberOfReviews < MinNumberOfReviews
                    ? new Tuple<bool, string>(false, "Number of reviews cannot be less than zero")
                    : new Tuple<bool, string>(true, null);
        };

        private Func<decimal, Tuple<bool, string>> salaryValidator = salary =>
        {
            return Program.isCustomValidationRules
                ? salary < MinValueOfSalaryCustom
                    ? new Tuple<bool, string>(false, "Salary is too small. It must be greater than 200")
                    : new Tuple<bool, string>(true, null)
                : salary < MinValueOfSalary
                    ? new Tuple<bool, string>(false, "Salary cannot be less than zero")
                    : new Tuple<bool, string>(true, null);
        };

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.Equals("create", request.Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Create(request.Parameters);
                return;
            }
            else
            {
                if (this.NextHandler != null)
                {
                    this.NextHandler.Handle(request);
                }
                else
                {
                    this.PrintMissedCommandInfo(request.Command);
                }
            }
        }

        private void Create(string parameters)
        {
            Console.Write("First name: ");
            var firstName = this.ReadInput(this.stringConverter, this.firstNameValidator);

            Console.Write("Last name: ");
            var lastName = this.ReadInput(this.stringConverter, this.lastNameValidator);

            Console.Write("Date of birth: ");
            var dateOfBirth = this.ReadInput(this.dateConverter, this.dateOfBirthValidator);

            Console.Write("Sex: ");
            var sex = this.ReadInput(this.charConverter, this.sexValidator);

            Console.Write("Number of reviews: ");
            var numberOfReviews = this.ReadInput(this.numberOfReviewsConverter, this.numberOfReviewsValidator);

            Console.Write("Salary: ");
            var salary = this.ReadInput(this.salaryConverter, this.salaryValidator);

            var userInputData = new UnverifiedData(firstName, lastName, dateOfBirth, sex, numberOfReviews, salary);
            int id = Program.fileCabinetService.CreateRecord(userInputData);
            Console.WriteLine($"Record #{id} is created.");
        }

        private T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
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

        private void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }
    }
}

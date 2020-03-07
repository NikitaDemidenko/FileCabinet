using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Find command handler.</summary>
    /// <seealso cref="CommandHandlerBase" />
    public class FindCommandHandler : CommandHandlerBase
    {
        private readonly IFileCabinetService fileCabinetService;

        /// <summary>Initializes a new instance of the <see cref="FindCommandHandler"/> class.</summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        /// <exception cref="ArgumentNullException">Thrown when fileCabinetService
        /// is null.</exception>
        public FindCommandHandler(IFileCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService ?? throw new ArgumentNullException(nameof(fileCabinetService));
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.Equals("find", request.Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Find(request.Parameters);
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

        private void Find(string parameters)
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
                var searchResult = this.fileCabinetService.FindByFirstName(propertyValue);
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
                var searchResult = this.fileCabinetService.FindByLastName(propertyValue);
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
                    var searchResult = this.fileCabinetService.FindByDateOfBirth(dateOfBirth);
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

        private void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }
    }
}

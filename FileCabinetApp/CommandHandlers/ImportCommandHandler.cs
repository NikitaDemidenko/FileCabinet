using System;
using System.IO;
using System.Text;
using System.Xml;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Import command handler.</summary>
    /// <seealso cref="ServiceCommandHandlerBase" />
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>Initializes a new instance of the <see cref="ImportCommandHandler"/> class.</summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        /// <exception cref="ArgumentNullException">Thrown when fileCabinetService
        /// is null.</exception>
        public ImportCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <exception cref="ArgumentNullException">Thrown when request
        /// is null.</exception>
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.Equals(ImportCommand, request.Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Import(request.Parameters);
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
                    PrintMissedCommandInfo(request.Command);
                }
            }
        }

        private void Import(string parameters)
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

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Import error: file {filePath} is not exist.");
                return;
            }

            if (typeOfFile.Equals(CsvFileExtension, StringComparison.InvariantCultureIgnoreCase))
            {
                using var reader = new StreamReader(filePath, Encoding.Unicode);
                var snapshot = new FileCabinetServiceSnapshot();
                try
                {
                    snapshot.LoadFromCsv(reader, this.fileCabinetService.Validator);
                }
                catch (FormatException)
                {
                    Console.WriteLine("One of the record's properties has invalid format.");
                    return;
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("One of the record's has invalid number of properties.");
                    return;
                }

                foreach (var record in snapshot.InvalidRecords)
                {
                    Console.WriteLine($"Record #{record.Item1.Id} was skipped: {record.Item2}");
                }

                this.fileCabinetService.Restore(snapshot);
                Console.WriteLine($"{snapshot.Records.Count} records were imported.");
            }
            else if (typeOfFile.Equals(XmlFileExtension, StringComparison.InvariantCultureIgnoreCase))
            {
                using var reader = XmlReader.Create(filePath);
                var snapshot = new FileCabinetServiceSnapshot();
                try
                {
                    snapshot.LoadFromXml(reader, this.fileCabinetService.Validator);
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("One of the record's properties has invalid format.");
                    return;
                }

                foreach (var record in snapshot.InvalidRecords)
                {
                    Console.WriteLine($"Record #{record.Item1.Id} was skipped: {record.Item2}");
                }

                this.fileCabinetService.Restore(snapshot);
                Console.WriteLine($"{snapshot.Records.Count} records were imported.");
            }
            else
            {
                Console.WriteLine("Invalid type of file.");
                return;
            }
        }
    }
}

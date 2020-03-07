using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Export command handler.</summary>
    /// <seealso cref="CommandHandlerBase" />
    public class ExportCommandHandler : CommandHandlerBase
    {
        private readonly IFileCabinetService fileCabinetService;

        /// <summary>Initializes a new instance of the <see cref="ExportCommandHandler"/> class.</summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        /// <exception cref="ArgumentNullException">Thrown when fileCabinetService
        /// is null.</exception>
        public ExportCommandHandler(IFileCabinetService fileCabinetService)
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

            if (string.Equals("export", request.Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Export(request.Parameters);
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

        private void Export(string parameters)
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
                    using var streamWriter = new StreamWriter(filePath, append, Encoding.Unicode);
                    var snapshot = this.fileCabinetService.MakeSnapshot();
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

        private void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }
    }
}

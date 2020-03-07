using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Help command handler.</summary>
    /// <seealso cref="CommandHandlerBase" />
    public class HelpCommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private readonly string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "create", "creates new record", "The 'create' command creates new record." },
            new string[] { "edit", "edits a record by Id", "The 'edit' command edits a record by Id." },
            new string[] { "find", "finds records by selected property", "The 'find' command finds records by selected property" },
            new string[] { "remove", "removes record by id", "The 'remove' command removes record by id" },
            new string[] { "purge", "defragments the data file (file system only)", "The 'purge' command defragments the data file" },
            new string[] { "list", "prints all records", "The 'list' command prints the records." },
            new string[] { "stat", "shows the number of records", "The 'stat' command shows the number of records." },
            new string[] { "export", "exports records to file", "The 'export' command exports records to file." },
            new string[] { "import", "imports records from file", "The 'import' command imports records from file." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
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

            if (string.Equals("help", request.Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.PrintHelp(request.Parameters);
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

        private void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(this.helpMessages, 0, this.helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(this.helpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in this.helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }
    }
}

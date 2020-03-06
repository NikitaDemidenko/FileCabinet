using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>List command handler.</summary>
    /// <seealso cref="CommandHandlerBase" />
    public class ListCommandHandler : CommandHandlerBase
    {
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

            if (string.Equals("list", request.Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.List(request.Parameters);
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

        private void List(string parameters)
        {
            if (Program.fileCabinetService.GetStat() == 0)
            {
                Console.WriteLine("There are no records.");
                return;
            }

            var records = Program.fileCabinetService.GetRecords();
            foreach (var record in records)
            {
                Console.WriteLine(record);
            }
        }

        private void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }
    }
}

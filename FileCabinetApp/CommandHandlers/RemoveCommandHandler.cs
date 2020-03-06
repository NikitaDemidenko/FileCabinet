using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Remove command handler.</summary>
    /// <seealso cref="CommandHandlerBase" />
    public class RemoveCommandHandler : CommandHandlerBase
    {
        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.Equals("remove", request.Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Remove(request.Parameters);
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

        private void Remove(string parameters)
        {
            if (!int.TryParse(parameters, out int id))
            {
                Console.WriteLine("Invalid characters.");
                Console.WriteLine();
                return;
            }

            if (!Program.fileCabinetService.StoredIdentifiers.Contains(id))
            {
                Console.WriteLine($"Record #{id} doesn't exist.");
                Console.WriteLine();
                return;
            }

            Program.fileCabinetService.RemoveRecord(id);
            Console.WriteLine($"Record #{id} is removed.");
        }

        private void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }
    }
}

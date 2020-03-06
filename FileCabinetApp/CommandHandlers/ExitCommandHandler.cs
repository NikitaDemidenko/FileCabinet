using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Exit command handler.</summary>
    /// <seealso cref="CommandHandlerBase" />
    public class ExitCommandHandler : CommandHandlerBase
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

            if (string.Equals("exit", request.Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Exit(request.Parameters);
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

        private void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            Program.isRunning = false;
        }

        private void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }
    }
}

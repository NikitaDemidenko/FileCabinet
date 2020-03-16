using System;
using static FileCabinetApp.ConstantsAndValidationRulesSettings.Constants;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Exit command handler.</summary>
    /// <seealso cref="CommandHandlerBase" />
    public class ExitCommandHandler : CommandHandlerBase
    {
        private readonly Action<bool> setProgramStatus;

        /// <summary>Initializes a new instance of the <see cref="ExitCommandHandler"/> class.</summary>
        /// <param name="setProgramStatus">Sets program status.</param>
        /// <exception cref="ArgumentNullException">Thrown when setProgramStatus
        /// is null.</exception>
        public ExitCommandHandler(Action<bool> setProgramStatus)
        {
            this.setProgramStatus = setProgramStatus ?? throw new ArgumentNullException(nameof(setProgramStatus));
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

            if (string.Equals(ExitCommand, request.Command, StringComparison.InvariantCultureIgnoreCase))
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
                    PrintMissedCommandInfo(request.Command);
                }
            }
        }

        private void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            bool isRunning = false;
            this.setProgramStatus(isRunning);
        }
    }
}

using System;
using static FileCabinetApp.ConstantsAndValidationRulesSettings.Constants;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Remove command handler.</summary>
    /// <seealso cref="ServiceCommandHandlerBase" />
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.</summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        /// <exception cref="ArgumentNullException">Thrown when fileCabinetService
        /// is null.</exception>
        public RemoveCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
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

            if (string.Equals(RemoveCommand, request.Command, StringComparison.InvariantCultureIgnoreCase))
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
                    PrintMissedCommandInfo(request.Command);
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

            if (!this.fileCabinetService.StoredIdentifiers.Contains(id))
            {
                Console.WriteLine($"Record #{id} doesn't exist.");
                Console.WriteLine();
                return;
            }

            this.fileCabinetService.RemoveRecord(id);
            Console.WriteLine($"Record #{id} is removed.");
        }
    }
}

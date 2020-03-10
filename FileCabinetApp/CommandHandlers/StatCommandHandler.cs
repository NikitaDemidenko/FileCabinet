using System;
using static FileCabinetApp.ConstantsAndValidationRulesSettings.Constants;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Stat command handler.</summary>
    /// <seealso cref="ServiceCommandHandlerBase" />
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>Initializes a new instance of the <see cref="StatCommandHandler"/> class.</summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        /// <exception cref="ArgumentNullException">Thrown when fileCabinetService
        /// is null.</exception>
        public StatCommandHandler(IFileCabinetService fileCabinetService)
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

            if (string.Equals(StatCommand, request.Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Stat(request.Parameters);
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

        private void Stat(string parameters)
        {
            var recordsCount = this.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
            Console.WriteLine($"{this.fileCabinetService.AllRecordsCount - recordsCount} record(s) has been deleted.");
        }
    }
}

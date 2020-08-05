using System;
using static FileCabinetApp.ConstantsAndValidationRulesSettings.Constants;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Purge command handler.</summary>
    /// <seealso cref="ServiceCommandHandlerBase" />
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.</summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        /// <exception cref="ArgumentNullException">Thrown when fileCabinetService
        /// is null.</exception>
        public PurgeCommandHandler(IFileCabinetService fileCabinetService)
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

            if (string.Equals(PurgeCommand, request.Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Purge(request.Parameters);
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

        private void Purge(string parameters)
        {
            try
            {
                this.fileCabinetService.Purge();
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("This command works with file system only.");
                return;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            int allRecordsCountBeforePurge = this.fileCabinetService.AllRecordsCount;
            int purgedRecordsCount = allRecordsCountBeforePurge - this.fileCabinetService.GetStat();
            Console.WriteLine($"Data file processing is completed: {purgedRecordsCount} of {allRecordsCountBeforePurge} records were purged.");
        }
    }
}

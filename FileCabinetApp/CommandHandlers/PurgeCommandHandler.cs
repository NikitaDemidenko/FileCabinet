using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Purge command handler.</summary>
    /// <seealso cref="CommandHandlerBase" />
    public class PurgeCommandHandler : CommandHandlerBase
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

            if (string.Equals("purge", request.Command, StringComparison.InvariantCultureIgnoreCase))
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
                    this.PrintMissedCommandInfo(request.Command);
                }
            }
        }

        private void Purge(string parameters)
        {
            if (Program.fileCabinetService is FileCabinetFilesystemService fileCabinetFilesystemService)
            {
                try
                {
                    fileCabinetFilesystemService.Purge();
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }

                int allRecordsCountBeforePurge = fileCabinetFilesystemService.AllRecordsCount;
                int purgedRecordsCount = allRecordsCountBeforePurge - fileCabinetFilesystemService.GetStat();
                Console.WriteLine($"Data file processing is completed: {purgedRecordsCount} of {allRecordsCountBeforePurge} records were purged.");
            }
            else
            {
                Console.WriteLine("This command works with file system only.");
                return;
            }
        }

        private void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }
    }
}

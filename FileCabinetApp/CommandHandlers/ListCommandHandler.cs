using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>List command handler.</summary>
    /// <seealso cref="ServiceCommandHandlerBase" />
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>Initializes a new instance of the <see cref="ListCommandHandler"/> class.</summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        /// <exception cref="ArgumentNullException">Thrown when fileCabinetService
        /// is null.</exception>
        public ListCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

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
                    PrintMissedCommandInfo(request.Command);
                }
            }
        }

        private void List(string parameters)
        {
            if (this.fileCabinetService.GetStat() == 0)
            {
                Console.WriteLine("There are no records.");
                return;
            }

            var records = this.fileCabinetService.GetRecords();
            foreach (var record in records)
            {
                Console.WriteLine(record);
            }
        }
    }
}

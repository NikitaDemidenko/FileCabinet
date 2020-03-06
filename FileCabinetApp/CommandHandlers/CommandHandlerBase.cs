using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Command handler base.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.ICommandHandler" />
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler nextHandler;

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        public abstract void Handle(AppCommandRequest request);

        /// <summary>Sets the next handler.</summary>
        /// <param name="handler">The handler.</param>
        /// <exception cref="ArgumentNullException">Thrown when handler is null.</exception>
        public void SetNext(ICommandHandler handler)
        {
            this.nextHandler = handler ?? throw new ArgumentNullException(nameof(handler));
        }
    }
}

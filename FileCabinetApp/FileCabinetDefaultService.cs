using System;
using static FileCabinetApp.Constants;

namespace FileCabinetApp
{
    /// <summary>Provides methods for interaction with records in the file cabinet using default validation rules.</summary>
    /// <seealso cref="FileCabinetApp.FileCabinetService" />
    public sealed class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>Initializes a new instance of the <see cref="FileCabinetDefaultService"/> class.</summary>
        public FileCabinetDefaultService()
            : base(new DefaultValidator())
        {
        }
    }
}

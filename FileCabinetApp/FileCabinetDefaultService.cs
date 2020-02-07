using System;
using static FileCabinetApp.Constants;

namespace FileCabinetApp
{
    /// <summary>Provides methods for interaction with records in the file cabinet using default validation rules.</summary>
    /// <seealso cref="FileCabinetApp.FileCabinetService" />
    public sealed class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>Creates default validator.</summary>
        /// <returns>Returns new <see cref="IRecordValidator"/> object.</returns>
        protected override IRecordValidator CreateValidator()
        {
            return new DefaultValidator();
        }
    }
}

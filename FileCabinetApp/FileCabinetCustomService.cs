using System;
using System.Text.RegularExpressions;
using static FileCabinetApp.Constants;

namespace FileCabinetApp
{
    /// <summary>Provides methods for interaction with records in the file cabinet using custom validation rules.</summary>
    /// <seealso cref="FileCabinetApp.FileCabinetService" />
    public sealed class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>Creates custom validator.</summary>
        /// <returns>Returns new <see cref="IRecordValidator"/> object.</returns>
        protected override IRecordValidator CreateValidator()
        {
            return new CustomValidator();
        }
    }
}

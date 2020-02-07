using System;
using System.Text.RegularExpressions;
using static FileCabinetApp.Constants;

namespace FileCabinetApp
{
    /// <summary>Provides methods for interaction with records in the file cabinet using custom validation rules.</summary>
    /// <seealso cref="FileCabinetApp.FileCabinetService" />
    public sealed class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>Initializes a new instance of the <see cref="FileCabinetCustomService"/> class.</summary>
        public FileCabinetCustomService()
            : base(new CustomValidator())
        {
        }
    }
}

using System;

namespace FileCabinetGenerator
{
    /// <summary>Command line args for FileCabinetGenerator.</summary>
    [Flags]
    public enum Flags
    {
        /// <summary>Default.</summary>
        Default = 0,

        /// <summary>The output type flag.</summary>
        OutputType = 1,

        /// <summary>The output flag.</summary>
        Output = 2,

        /// <summary>The records amount flag.</summary>
        RecordsAmount = 4,

        /// <summary>The start identifier flag.</summary>
        StartId = 8,
    }
}

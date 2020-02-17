using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>Command line flags.</summary>
    [Flags]
    public enum Flags
    {
        /// <summary>Default value.</summary>
        Default = 0,

        /// <summary>Validation rules flag.</summary>
        ValidationRules = 1,

        /// <summary>Storage flag.</summary>
        Storage = 2,
    }
}

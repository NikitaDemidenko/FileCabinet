using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetGenerator
{
    [Flags]
    public enum Flags
    {
        Default = 0,
        OutputType = 1,
        Output = 2,
        RecordsAmount = 4,
        StartId = 8,
    }
}

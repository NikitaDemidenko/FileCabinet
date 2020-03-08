using System;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Sex validator.</summary>
    /// <seealso cref="IRecordValidator" />
    public class SexValidator : IRecordValidator
    {
        /// <summary>Validates user input data.</summary>
        /// <param name="data">Data to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="ArgumentException">Wrong sex.</exception>
        public void ValidateParameters(UnverifiedData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Sex != MaleSex && data.Sex != FemaleSex)
            {
                throw new ArgumentException("Wrong sex.");
            }
        }
    }
}

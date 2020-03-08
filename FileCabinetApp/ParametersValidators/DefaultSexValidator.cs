using System;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Default sex validator.</summary>
    /// <seealso cref="IRecordValidator" />
    public class DefaultSexValidator : IRecordValidator
    {
        /// <summary>Validates user input data.</summary>
        /// <param name="parameters">Parameters to validate.</param>
        /// <exception cref="ArgumentException">Wrong sex
        /// or
        /// parameters isn't char.</exception>
        public void ValidateParameters(object parameters)
        {
            if (parameters is char sex)
            {
                if (sex != MaleSex && sex != FemaleSex)
                {
                    throw new ArgumentException("Wrong sex.");
                }
            }
            else
            {
                throw new ArgumentException($"{nameof(parameters)} must be char.");
            }
        }
    }
}

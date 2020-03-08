using System;
using System.Collections.Generic;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Composite validator.</summary>
    /// <seealso cref="IRecordValidator" />
    public abstract class CompositeValidator : IRecordValidator
    {
        private readonly List<IRecordValidator> validators = new List<IRecordValidator>();

        /// <summary>Initializes a new instance of the <see cref="CompositeValidator"/> class.</summary>
        /// <param name="validators">Validators.</param>
        /// <exception cref="ArgumentNullException">Thrown when validators
        /// is null.</exception>
        protected CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            if (validators == null)
            {
                throw new ArgumentNullException(nameof(validators));
            }

            foreach (var validator in validators)
            {
                this.validators.Add(validator);
            }
        }

        /// <summary>Validates user input data.</summary>
        /// <param name="data">Data to validate.</param>
        public void ValidateParameters(UnverifiedData data)
        {
            foreach (var validator in this.validators)
            {
                validator.ValidateParameters(data);
            }
        }
    }
}

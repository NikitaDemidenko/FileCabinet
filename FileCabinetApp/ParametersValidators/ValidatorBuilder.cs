using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Validator builder.</summary>
    public class ValidatorBuilder
    {
        private readonly List<IRecordValidator> validators = new List<IRecordValidator>();

        /// <summary>Validates the first name.</summary>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="isCustomValidationRules">if set to <c>true</c> then validation rules are custom.</param>
        /// <returns>Returns <see cref="ValidatorBuilder"/> object.</returns>
        public ValidatorBuilder ValidateFirstName(int minLength, int maxLength, bool isCustomValidationRules)
        {
            this.validators.Add(new FirstNameValidator(minLength, maxLength, isCustomValidationRules));
            return this;
        }

        /// <summary>Validates the last name.</summary>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="isCustomValidationRules">if set to <c>true</c> then validation rules are custom.</param>
        /// <returns>Returns <see cref="ValidatorBuilder"/> object.</returns>
        public ValidatorBuilder ValidateLastName(int minLength, int maxLength, bool isCustomValidationRules)
        {
            this.validators.Add(new LastNameValidator(minLength, maxLength, isCustomValidationRules));
            return this;
        }

        /// <summary>Validates the date of birth.</summary>
        /// <param name="from">From data.</param>
        /// <param name="to">To date.</param>
        /// <returns>Returns <see cref="ValidatorBuilder"/> object.</returns>
        public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
            return this;
        }

        /// <summary>Validates the sex.</summary>
        /// <returns>Returns <see cref="ValidatorBuilder"/> object.</returns>
        public ValidatorBuilder ValidateSex()
        {
            this.validators.Add(new SexValidator());
            return this;
        }

        /// <summary>Validates the number of reviews.</summary>
        /// <param name="minNumber">The minimum number.</param>
        /// <returns>Returns <see cref="ValidatorBuilder"/> object.</returns>
        public ValidatorBuilder ValidateNumberOfReviews(short minNumber)
        {
            this.validators.Add(new NumberOfReviewsValidator(minNumber));
            return this;
        }

        /// <summary>Validates the salary.</summary>
        /// <param name="minSalary">The minimum salary.</param>
        /// <returns>Returns <see cref="ValidatorBuilder"/> object.</returns>
        public ValidatorBuilder ValidateSalary(decimal minSalary)
        {
            this.validators.Add(new SalaryValidator(minSalary));
            return this;
        }

        /// <summary>Creates <see cref="CompositeValidator"/> instance.</summary>
        /// <returns>Returns <see cref="CompositeValidator"/> object.</returns>
        public IRecordValidator Create() => new CompositeValidator(this.validators);
    }
}

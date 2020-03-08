using System;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Performs default validation on user input data.</summary>
    /// <seealso cref="IRecordValidator" />
    public sealed class DefaultValidator : CompositeValidator
    {
        /// <summary>Initializes a new instance of the <see cref="DefaultValidator"/> class.</summary>
        public DefaultValidator()
            : base(new IRecordValidator[]
        {
            new FirstNameValidator(MinNumberOfSymbols, MaxNumberOfSymbols, false),
            new LastNameValidator(MinNumberOfSymbols, MaxNumberOfSymbols, false),
            new DateOfBirthValidator(MinDateOfBirth, DateTime.Now),
            new SexValidator(),
            new NumberOfReviewsValidator(MinNumberOfReviews),
            new SalaryValidator(MinValueOfSalary),
        })
        {
        }
    }
}

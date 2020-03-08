using System;
using static FileCabinetApp.Constants;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Performs custom validation on user input data.</summary>
    /// <seealso cref="IRecordValidator" />
    public sealed class CustomValidator : CompositeValidator
    {
        /// <summary>Initializes a new instance of the <see cref="CustomValidator"/> class.</summary>
        public CustomValidator()
            : base(new IRecordValidator[]
        {
            new FirstNameValidator(MinNumberOfSymbols, MaxNumberOfSymbols, true),
            new LastNameValidator(MinNumberOfSymbols, MaxNumberOfSymbols, true),
            new DateOfBirthValidator(MinDateOfBirthCustom, DateTime.Now),
            new SexValidator(),
            new NumberOfReviewsValidator(MinNumberOfReviewsCustom),
            new SalaryValidator(MinValueOfSalaryCustom),
        })
        {
        }
    }
}

using System;
using static FileCabinetApp.ConstantsAndValidationRulesSettings.Constants;

namespace FileCabinetApp.ParametersValidators
{
    /// <summary>Provides extension methods for <see cref="ValidatorBuilder"/> class.</summary>
    public static class ValidatorBuilderExtension
    {
        /// <summary>Creates default validator.</summary>
        /// <param name="builder">Builder.</param>
        /// <returns>Returns <see cref="IRecordValidator"/> object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when builder is null.</exception>
        public static IRecordValidator CreateDefault(this ValidatorBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder
                .ValidateFirstName(MinFirstNameLength, MaxFirstNameLength, false)
                .ValidateLastName(MinLastNameLength, MaxLastNameLength, false)
                .ValidateDateOfBirth(MinDateOfBirth, MaxDateOfBirth)
                .ValidateSex()
                .ValidateNumberOfReviews(MinNumberOfReviews)
                .ValidateSalary(MinValueOfSalary)
                .Create();
        }

        /// <summary>Creates custom validator.</summary>
        /// <param name="builder">Builder.</param>
        /// <returns>Returns <see cref="IRecordValidator"/> object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when builder is null.</exception>
        public static IRecordValidator CreateCustom(this ValidatorBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder
                .ValidateFirstName(MinFirstNameLengthCustom, MaxFirstNameLengthCustom, true)
                .ValidateLastName(MinLastNameLengthCustom, MaxLastNameLengthCustom, true)
                .ValidateDateOfBirth(MinDateOfBirthCustom, MaxDateOfBirthCustom)
                .ValidateSex()
                .ValidateNumberOfReviews(MinNumberOfReviewsCustom)
                .ValidateSalary(MinValueOfSalaryCustom)
                .Create();
        }
    }
}

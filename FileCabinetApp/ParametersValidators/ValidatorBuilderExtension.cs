using System;
using static FileCabinetApp.Constants;

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
                .ValidateFirstName(MinNumberOfSymbols, MaxNumberOfSymbols, false)
                .ValidateLastName(MinNumberOfSymbols, MaxNumberOfSymbols, false)
                .ValidateDateOfBirth(MinDateOfBirth, DateTime.Now)
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
                .ValidateFirstName(MinNumberOfSymbols, MaxNumberOfSymbols, true)
                .ValidateLastName(MinNumberOfSymbols, MaxNumberOfSymbols, true)
                .ValidateDateOfBirth(MinDateOfBirthCustom, DateTime.Now)
                .ValidateSex()
                .ValidateNumberOfReviews(MinNumberOfReviewsCustom)
                .ValidateSalary(MinValueOfSalaryCustom)
                .Create();
        }
    }
}

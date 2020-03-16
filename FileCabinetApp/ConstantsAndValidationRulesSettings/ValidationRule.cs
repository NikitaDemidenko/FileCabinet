using System;
using System.Text.Json.Serialization;

namespace FileCabinetApp.ConstantsAndValidationRulesSettings
{
    /// <summary>Validation rule.</summary>
    public class ValidationRule
    {
        /// <summary>Gets or sets the first name limits.</summary>
        /// <value>The first name limits.</value>
        [JsonPropertyName("firstName")]
        public FirstNameLimits FirstNameLimits { get; set; }

        /// <summary>Gets or sets the last name limits.</summary>
        /// <value>The last name limits.</value>
        [JsonPropertyName("lastName")]
        public LastNameLimits LastNameLimits { get; set; }

        /// <summary>Gets or sets the date of birth limits.</summary>
        /// <value>The date of birth limits.</value>
        [JsonPropertyName("dateOfBirth")]
        public DateOfBirthLimits DateOfBirthLimits { get; set; }

        /// <summary>Gets or sets the number of reviews limits.</summary>
        /// <value>The number of reviews limits.</value>
        [JsonPropertyName("numberOfReviews")]
        public NumberOfReviewsLimits NumberOfReviewsLimits { get; set; }

        /// <summary>Gets or sets the salary limits.</summary>
        /// <value>The salary limits.</value>
        [JsonPropertyName("salary")]
        public SalaryLimits SalaryLimits { get; set; }
    }
}

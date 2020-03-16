using System;
using System.Text.Json.Serialization;

namespace FileCabinetApp.ConstantsAndValidationRulesSettings
{
    /// <summary>Number of reviews limits.</summary>
    public class NumberOfReviewsLimits
    {
        /// <summary>Gets or sets the minimum number of reviews.</summary>
        /// <value>The minimum number of reviews.</value>
        [JsonPropertyName("minNumber")]
        public short MinNumber { get; set; }
    }
}

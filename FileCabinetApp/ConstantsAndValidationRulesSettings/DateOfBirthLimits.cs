using System;
using System.Text.Json.Serialization;

namespace FileCabinetApp.ConstantsAndValidationRulesSettings
{
    /// <summary>Date of birth limits.</summary>
    public class DateOfBirthLimits
    {
        /// <summary>Gets or sets from date.</summary>
        /// <value>From date.</value>
        [JsonPropertyName("from")]
        public string From { get; set; }

        /// <summary>Gets or sets to date.</summary>
        /// <value>To date.</value>
        [JsonPropertyName("to")]
        public string To { get; set; }
    }
}

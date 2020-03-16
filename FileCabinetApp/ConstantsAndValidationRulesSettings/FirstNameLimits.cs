using System;
using System.Text.Json.Serialization;

namespace FileCabinetApp.ConstantsAndValidationRulesSettings
{
    /// <summary>First name limits.</summary>
    public class FirstNameLimits
    {
        /// <summary>Gets or sets the minimum length.</summary>
        /// <value>The minimum length.</value>
        [JsonPropertyName("min")]
        public int MinLength { get; set; }

        /// <summary>Gets or sets the maximum length.</summary>
        /// <value>The maximum length.</value>
        [JsonPropertyName("max")]
        public int MaxLength { get; set; }
    }
}

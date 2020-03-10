using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FileCabinetApp.ConstantsAndValidationRulesSettings
{
    /// <summary>Last name limits.</summary>
    public class LastNameLimits
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

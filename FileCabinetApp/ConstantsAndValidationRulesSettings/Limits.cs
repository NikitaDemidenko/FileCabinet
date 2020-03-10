using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FileCabinetApp.ConstantsAndValidationRulesSettings
{
    /// <summary>Limits.</summary>
    public class Limits
    {
        /// <summary>Gets or sets the default validation rules.</summary>
        /// <value>The default validation rules.</value>
        [JsonPropertyName("default")]
        public ValidationRule Default { get; set; }

        /// <summary>Gets or sets the custom validation rules.</summary>
        /// <value>The custom validation rules.</value>
        [JsonPropertyName("custom")]
        public ValidationRule Custom { get; set; }
    }
}

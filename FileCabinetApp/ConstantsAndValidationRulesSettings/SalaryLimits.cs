using System;
using System.Text.Json.Serialization;

namespace FileCabinetApp.ConstantsAndValidationRulesSettings
{
    /// <summary>Salary limits.</summary>
    public class SalaryLimits
    {
        /// <summary>Gets or sets the minimum salary.</summary>
        /// <value>The minimum salary.</value>
        [JsonPropertyName("minSalary")]
        public decimal MinSalary { get; set; }
    }
}

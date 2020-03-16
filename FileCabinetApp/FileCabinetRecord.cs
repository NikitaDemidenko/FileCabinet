using System;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;
using static FileCabinetApp.ConstantsAndValidationRulesSettings.Constants;

namespace FileCabinetApp
{
    /// <summary>Provides properties for records.</summary>
    public class FileCabinetRecord
    {
        /// <summary>Initializes a new instance of the <see cref="FileCabinetRecord"/> class.</summary>
        public FileCabinetRecord()
        {
        }

        /// <summary>Gets or sets the identifier.</summary>
        /// <value>Identifier of record.</value>
        [XmlAttribute("id")]
        public int Id { get; set; }

        /// <summary>Gets or sets the full name.</summary>
        /// <value>Full name.</value>
        [XmlElement("name")]
        public FullName Name { get; set; }

        /// <summary>Gets or sets the date of birth.</summary>
        /// <value>Date of birth.</value>
        [XmlElement("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>Gets or sets the sex.</summary>
        /// <value>Sex.</value>
        [XmlElement("sex")]
        public char Sex { get; set; }

        /// <summary>Gets or sets the number of reviews.</summary>
        /// <value>Number of reviews.</value>
        [XmlElement("numberOfReviews")]
        public short NumberOfReviews { get; set; }

        /// <summary>Gets or sets the salary.</summary>
        /// <value>Salary.</value>
        [XmlElement("salary")]
        public decimal Salary { get; set; }

        /// <summary>Converts file cabinet record to its string representation.</summary>
        /// <returns>A <see cref="string"/> that represents <see cref="FileCabinetRecord"/> instance.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"#{this.Id}, ");
            builder.Append($"{this.Name.FirstName}, ");
            builder.Append($"{this.Name.LastName}, ");
            builder.Append($"{this.DateOfBirth.ToString(OutputDateFormat, CultureInfo.InvariantCulture)}, ");
            builder.Append($"{this.Sex}, ");
            builder.Append($"{this.NumberOfReviews}, ");
            builder.Append($"{this.Salary.ToString(CurrencyFormat, Culture)}");
            return builder.ToString();
        }
    }
}

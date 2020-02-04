using System;
using System.Globalization;
using System.Text;
using static FileCabinetApp.Constants;

namespace FileCabinetApp
{
    /// <summary>Provides properties for records.</summary>
    public class FileCabinetRecord
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>Identifier of record.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the first name.</summary>
        /// <value>First name.</value>
        public string FirstName { get; set; }

        /// <summary>Gets or sets the last name.</summary>
        /// <value>Last name.</value>
        public string LastName { get; set; }

        /// <summary>Gets or sets the date of birth.</summary>
        /// <value>Date of birth.</value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>Gets or sets the sex.</summary>
        /// <value>Sex.</value>
        public char Sex { get; set;  }

        /// <summary>Gets or sets the number of reviews.</summary>
        /// <value>Number of reviews.</value>
        public short NumberOfReviews { get; set; }

        /// <summary>Gets or sets the salary.</summary>
        /// <value>Salary.</value>
        public decimal Salary { get; set; }

        /// <summary>Converts file cabinet record to its string representation.</summary>
        /// <returns>A <see cref="string"/> that represents <see cref="FileCabinetRecord"/> instance.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"#{this.Id}, ");
            builder.Append($"{this.FirstName}, ");
            builder.Append($"{this.LastName}, ");
            builder.Append($"{this.DateOfBirth.ToString(OutputDateFormat, CultureInfo.InvariantCulture)}, ");
            builder.Append($"{this.Sex}, ");
            builder.Append($"{this.NumberOfReviews}, ");
            builder.Append($"{this.Salary.ToString(CurrencyFormat, Culture)}");
            return builder.ToString();
        }
    }
}

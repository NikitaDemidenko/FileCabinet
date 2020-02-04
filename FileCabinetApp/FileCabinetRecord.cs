using System;
using System.Globalization;
using System.Text;
using static FileCabinetApp.Constants;

namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public char Sex { get; set;  }

        public short NumberOfReviews { get; set; }

        public decimal Salary { get; set; }

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

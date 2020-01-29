using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, char sex, short numberOfReviews, decimal salary)
        {
            if (firstName == null)
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            if (lastName == null)
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            if (firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("First name's length is out of range.");
            }

            if (lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("Last name's length is out of range.");
            }

            if (dateOfBirth <= new DateTime(1950, 01, 01) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("Invalid date.");
            }

            if (sex != 'M' && sex != 'F')
            {
                throw new ArgumentException("There're only two genders.");
            }

            if (salary < 0)
            {
                throw new ArgumentException("Salary cannot be less than zero.");
            }

            if (numberOfReviews < 0)
            {
                throw new ArgumentException("Number of reviews cannot be less than zero.");
            }

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Sex = sex,
                NumberOfReviews = numberOfReviews,
                Salary = salary,
            };

            this.list.Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords() => this.list.ToArray();

        public int GetStat() => this.list.Count;

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char sex, short numberOfReviews, decimal salary)
        {
            if (id < 1 || id > this.list.Count)
            {
                throw new ArgumentException($"There is no #{id} record.");
            }

            if (firstName == null)
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            if (lastName == null)
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            if (firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("First name's length is out of range.");
            }

            if (lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("Last name's length is out of range.");
            }

            if (dateOfBirth <= new DateTime(1950, 01, 01) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("Invalid date.");
            }

            if (sex != 'M' && sex != 'F')
            {
                throw new ArgumentException("There're only two genders.");
            }

            if (salary < 0)
            {
                throw new ArgumentException("Salary cannot be less than zero.");
            }

            if (numberOfReviews < 0)
            {
                throw new ArgumentException("Number of reviews cannot be less than zero.");
            }

            foreach (var record in this.list)
            {
                if (record.Id == id)
                {
                    record.FirstName = firstName;
                    record.LastName = lastName;
                    record.DateOfBirth = dateOfBirth;
                    record.Sex = sex;
                    record.NumberOfReviews = numberOfReviews;
                    record.Salary = salary;
                    return;
                }
            }
        }
    }
}

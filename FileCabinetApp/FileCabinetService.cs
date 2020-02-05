using System;
using System.Collections.Generic;
using static FileCabinetApp.Constants;

namespace FileCabinetApp
{
    /// <summary>Provides methods for interaction with records in the file cabinet.</summary>
    public abstract class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        /// <summary>Creates new <see cref="FileCabinetRecord"/> instance.</summary>
        /// <param name="userInputData">User input data.</param>
        /// <returns>Returns new <see cref="FileCabinetRecord"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <em>userInput</em> is <em>null</em>.</exception>
        public int CreateRecord(UserInputData userInputData)
        {
            if (userInputData == null)
            {
                throw new ArgumentNullException(nameof(userInputData));
            }

            this.ValidateParameters(userInputData);
            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = userInputData.FirstName,
                LastName = userInputData.LastName,
                DateOfBirth = userInputData.DateOfBirth,
                Sex = userInputData.Sex,
                NumberOfReviews = userInputData.NumberOfReviews,
                Salary = userInputData.Salary,
            };
            this.list.Add(record);

            string firstNameKey = userInputData.FirstName.ToUpperInvariant();
            if (!this.firstNameDictionary.ContainsKey(firstNameKey))
            {
                this.firstNameDictionary.Add(firstNameKey, new List<FileCabinetRecord>());
            }

            this.firstNameDictionary[firstNameKey].Add(record);
            string lastNameKey = userInputData.LastName.ToUpperInvariant();
            if (!this.lastNameDictionary.ContainsKey(lastNameKey))
            {
                this.lastNameDictionary.Add(lastNameKey, new List<FileCabinetRecord>());
            }

            this.lastNameDictionary[lastNameKey].Add(record);

            if (!this.dateOfBirthDictionary.ContainsKey(userInputData.DateOfBirth))
            {
                this.dateOfBirthDictionary.Add(userInputData.DateOfBirth, new List<FileCabinetRecord>());
            }

            this.dateOfBirthDictionary[userInputData.DateOfBirth].Add(record);
            return record.Id;
        }

        /// <summary>Gets the records.</summary>
        /// <returns>Returns an array of records.</returns>
        public FileCabinetRecord[] GetRecords() => this.list.ToArray();

        /// <summary>Gets the stat of records in the file cabinet.</summary>
        /// <returns>Returns number of records.</returns>
        public int GetStat() => this.list.Count;

        /// <summary>Edits record by identifier.</summary>
        /// <param name="id">Identifier.</param>
        /// <param name="userInput">User input data.</param>
        /// <exception cref="ArgumentNullException">Thrown when <em>userInput </em>is null.</exception>
        /// <exception cref="ArgumentException">Thrown when identifier is invalid.</exception>
        public void EditRecord(int id, UserInputData userInput)
        {
            if (id < MinValueOfId || id > this.list.Count)
            {
                throw new ArgumentException($"There is no #{id} record.");
            }

            if (userInput == null)
            {
                throw new ArgumentNullException(nameof(userInput));
            }

            this.ValidateParameters(userInput);

            foreach (var record in this.list)
            {
                if (record.Id == id)
                {
                    this.firstNameDictionary[record.FirstName.ToUpperInvariant()].Remove(record);
                    this.lastNameDictionary[record.LastName.ToUpperInvariant()].Remove(record);
                    this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
                    record.FirstName = userInput.FirstName;
                    record.LastName = userInput.LastName;
                    record.DateOfBirth = userInput.DateOfBirth;
                    record.Sex = userInput.Sex;
                    record.NumberOfReviews = userInput.NumberOfReviews;
                    record.Salary = userInput.Salary;

                    string firstNameKey = userInput.FirstName.ToUpperInvariant();
                    if (!this.firstNameDictionary.ContainsKey(firstNameKey))
                    {
                        this.firstNameDictionary.Add(firstNameKey, new List<FileCabinetRecord>());
                    }

                    this.firstNameDictionary[firstNameKey].Add(record);
                    string lastNameKey = userInput.LastName.ToUpperInvariant();
                    if (!this.lastNameDictionary.ContainsKey(lastNameKey))
                    {
                        this.lastNameDictionary.Add(lastNameKey, new List<FileCabinetRecord>());
                    }

                    this.lastNameDictionary[lastNameKey].Add(record);

                    if (!this.dateOfBirthDictionary.ContainsKey(userInput.DateOfBirth))
                    {
                        this.dateOfBirthDictionary.Add(userInput.DateOfBirth, new List<FileCabinetRecord>());
                    }

                    this.dateOfBirthDictionary[userInput.DateOfBirth].Add(record);
                    return;
                }
            }
        }

        /// <summary>Finds records by first name.</summary>
        /// <param name="firstName">First name to find.</param>
        /// <returns>Returns an array of found records.</returns>
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (firstName == null)
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            string firstNameKey = firstName.ToUpperInvariant();
            return this.firstNameDictionary.ContainsKey(firstNameKey) && this.firstNameDictionary[firstNameKey].Count != 0 ?
                this.firstNameDictionary[firstNameKey].ToArray() : Array.Empty<FileCabinetRecord>();
        }

        /// <summary>Finds records by last name.</summary>
        /// <param name="lastName">Last name to find.</param>
        /// <returns>Returns an array of found records.</returns>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (lastName == null)
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            string lastNameKey = lastName.ToUpperInvariant();
            return this.lastNameDictionary.ContainsKey(lastNameKey) && this.lastNameDictionary[lastNameKey].Count != 0 ?
                this.lastNameDictionary[lastNameKey].ToArray() : Array.Empty<FileCabinetRecord>();
        }

        /// <summary>Finds records by date of birth.</summary>
        /// <param name="dateOfBirth">Date of birth to find.</param>
        /// <returns>Returns an array of found records.</returns>
        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            return this.dateOfBirthDictionary.ContainsKey(dateOfBirth) && this.dateOfBirthDictionary[dateOfBirth].Count != 0 ?
                this.dateOfBirthDictionary[dateOfBirth].ToArray() : Array.Empty<FileCabinetRecord>();
        }

        /// <summary>Validates user input parameters.</summary>
        /// <param name="userInputData">User input.</param>
        protected abstract void ValidateParameters(UserInputData userInputData);
    }
}

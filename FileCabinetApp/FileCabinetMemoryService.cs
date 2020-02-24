using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static FileCabinetApp.Constants;

namespace FileCabinetApp
{
    /// <summary>Provides methods for interaction with records in memory.</summary>
    /// <seealso cref="IFileCabinetService" />
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
        private readonly List<int> storedIdentifiers = new List<int>();

        /// <summary>Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.</summary>
        /// <param name="validator">Validator.</param>
        /// <exception cref="ArgumentNullException">Thrown when <em>validator</em> is <em>null</em>.</exception>
        public FileCabinetMemoryService(IRecordValidator validator)
        {
            this.Validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        /// <summary>Gets the collection of stored identifiers.</summary>
        /// <value>Collections of identifiers strored in the file cabinet service.</value>
        public ReadOnlyCollection<int> StoredIdentifiers => new ReadOnlyCollection<int>(this.storedIdentifiers);

        /// <summary>Gets the validator of this <see cref="FileCabinetMemoryService"/> object.</summary>
        /// <value>The validator.</value>
        public IRecordValidator Validator { get; }

        /// <summary>Creates new <see cref="FileCabinetRecord"/> instance.</summary>
        /// <param name="userInputData">User input data.</param>
        /// <returns>Returns identifier of the new <see cref="FileCabinetRecord"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <em>userInput</em> is <em>null</em>.</exception>
        public int CreateRecord(UnverifiedData userInputData)
        {
            if (userInputData == null)
            {
                throw new ArgumentNullException(nameof(userInputData));
            }

            this.Validator.ValidateParameters(userInputData);
            var record = new FileCabinetRecord
            {
                Id = this.storedIdentifiers.Count != 0 ? this.storedIdentifiers.Max() + 1 : MinValueOfId,
                Name = new FullName(userInputData.FirstName, userInputData.LastName),
                DateOfBirth = userInputData.DateOfBirth,
                Sex = userInputData.Sex,
                NumberOfReviews = userInputData.NumberOfReviews,
                Salary = userInputData.Salary,
            };

            this.storedIdentifiers.Add(record.Id);

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
        /// <returns>Returns a read-only collection  of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords() => new ReadOnlyCollection<FileCabinetRecord>(this.list);

        /// <summary>Gets the stat of records in the file cabinet.</summary>
        /// <returns>Returns number of records.</returns>
        public int GetStat() => this.list.Count;

        /// <summary>Edits record by identifier.</summary>
        /// <param name="id">Identifier.</param>
        /// <param name="unverifiedData">Raw data.</param>
        /// <exception cref="ArgumentNullException">Thrown when <em>userInput </em>is null.</exception>
        /// <exception cref="ArgumentException">Thrown when identifier is invalid.</exception>
        public void EditRecord(int id, UnverifiedData unverifiedData)
        {
            if (!this.storedIdentifiers.Contains(id))
            {
                throw new ArgumentException($"There is no #{id} record.");
            }

            if (unverifiedData == null)
            {
                throw new ArgumentNullException(nameof(unverifiedData));
            }

            this.Validator.ValidateParameters(unverifiedData);

            foreach (var record in this.list)
            {
                if (record.Id == id)
                {
                    this.firstNameDictionary[record.Name.FirstName.ToUpperInvariant()].Remove(record);
                    this.lastNameDictionary[record.Name.LastName.ToUpperInvariant()].Remove(record);
                    this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
                    record.Name.FirstName = unverifiedData.FirstName;
                    record.Name.LastName = unverifiedData.LastName;
                    record.DateOfBirth = unverifiedData.DateOfBirth;
                    record.Sex = unverifiedData.Sex;
                    record.NumberOfReviews = unverifiedData.NumberOfReviews;
                    record.Salary = unverifiedData.Salary;

                    string firstNameKey = unverifiedData.FirstName.ToUpperInvariant();
                    if (!this.firstNameDictionary.ContainsKey(firstNameKey))
                    {
                        this.firstNameDictionary.Add(firstNameKey, new List<FileCabinetRecord>());
                    }

                    this.firstNameDictionary[firstNameKey].Add(record);
                    string lastNameKey = unverifiedData.LastName.ToUpperInvariant();
                    if (!this.lastNameDictionary.ContainsKey(lastNameKey))
                    {
                        this.lastNameDictionary.Add(lastNameKey, new List<FileCabinetRecord>());
                    }

                    this.lastNameDictionary[lastNameKey].Add(record);

                    if (!this.dateOfBirthDictionary.ContainsKey(unverifiedData.DateOfBirth))
                    {
                        this.dateOfBirthDictionary.Add(unverifiedData.DateOfBirth, new List<FileCabinetRecord>());
                    }

                    this.dateOfBirthDictionary[unverifiedData.DateOfBirth].Add(record);
                    return;
                }
            }
        }

        /// <summary>Finds records by first name.</summary>
        /// <param name="firstName">First name to find.</param>
        /// <returns>Returns a read-only collection of found records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (firstName == null)
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            string firstNameKey = firstName.ToUpperInvariant();
            return this.firstNameDictionary.ContainsKey(firstNameKey) && this.firstNameDictionary[firstNameKey].Count != 0 ?
                new ReadOnlyCollection<FileCabinetRecord>(this.firstNameDictionary[firstNameKey]) : null;
        }

        /// <summary>Finds records by last name.</summary>
        /// <param name="lastName">Last name to find.</param>
        /// <returns>Returns a read-only collection of found records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (lastName == null)
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            string lastNameKey = lastName.ToUpperInvariant();
            return this.lastNameDictionary.ContainsKey(lastNameKey) && this.lastNameDictionary[lastNameKey].Count != 0 ?
                new ReadOnlyCollection<FileCabinetRecord>(this.lastNameDictionary[lastNameKey]) : null;
        }

        /// <summary>Finds records by date of birth.</summary>
        /// <param name="dateOfBirth">Date of birth to find.</param>
        /// <returns>Returns a read-only collection of found records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            return this.dateOfBirthDictionary.ContainsKey(dateOfBirth) && this.dateOfBirthDictionary[dateOfBirth].Count != 0 ?
                new ReadOnlyCollection<FileCabinetRecord>(this.dateOfBirthDictionary[dateOfBirth]) : null;
        }

        /// <summary>Makes snapshot of current <see cref="FileCabinetMemoryService"/> object state.</summary>
        /// <returns>Returns new <see cref="FileCabinetServiceSnapshot"/>.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot() => new FileCabinetServiceSnapshot(this.GetRecords());

        /// <summary>Restores the specified snapshot.</summary>
        /// <param name="snapshot">Snapshot.</param>
        /// <exception cref="ArgumentNullException">Thrown when snapshot is null.</exception>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            UnverifiedData data;
            foreach (var record in snapshot.Records)
            {
                if (this.ContainsId(record.Id))
                {
                    data = new UnverifiedData(record);
                    this.EditRecord(record.Id, data);
                }
                else
                {
                    this.AddRecord(record);
                }
            }
        }

        private bool ContainsId(int id)
        {
            foreach (var record in this.list)
            {
                if (record.Id == id)
                {
                    return true;
                }
            }

            return false;
        }

        private void AddRecord(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            this.storedIdentifiers.Add(record.Id);

            this.list.Add(record);

            string firstNameKey = record.Name.FirstName.ToUpperInvariant();
            if (!this.firstNameDictionary.ContainsKey(firstNameKey))
            {
                this.firstNameDictionary.Add(firstNameKey, new List<FileCabinetRecord>());
            }

            this.firstNameDictionary[firstNameKey].Add(record);
            string lastNameKey = record.Name.LastName.ToUpperInvariant();
            if (!this.lastNameDictionary.ContainsKey(lastNameKey))
            {
                this.lastNameDictionary.Add(lastNameKey, new List<FileCabinetRecord>());
            }

            this.lastNameDictionary[lastNameKey].Add(record);

            if (!this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                this.dateOfBirthDictionary.Add(record.DateOfBirth, new List<FileCabinetRecord>());
            }

            this.dateOfBirthDictionary[record.DateOfBirth].Add(record);
        }
    }
}

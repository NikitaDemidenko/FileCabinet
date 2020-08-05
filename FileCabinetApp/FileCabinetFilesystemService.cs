using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using static FileCabinetApp.ConstantsAndValidationRulesSettings.Constants;

namespace FileCabinetApp
{
    /// <summary>Provides methods for interaction with records in file system.</summary>
    /// <seealso cref="IFileCabinetService" />
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private readonly List<int> storedIdentifiers = new List<int>();
        private readonly Dictionary<string, List<long>> firstNamesOffsets = new Dictionary<string, List<long>>();
        private readonly Dictionary<string, List<long>> lastNamesOffsets = new Dictionary<string, List<long>>();
        private readonly Dictionary<DateTime, List<long>> dateOfBirthOffsets = new Dictionary<DateTime, List<long>>();
        private FileStream fileStream;
        private int undeletedRecordsCount;
        private int allRecordsCount;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.</summary>
        /// <param name="fileStream">File stream.</param>
        /// <param name="validator">Validator.</param>
        /// <exception cref="ArgumentNullException">Thrown when fileStream or validator is null.</exception>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream ?? throw new ArgumentNullException(nameof(fileStream));
            this.Validator = validator ?? throw new ArgumentNullException(nameof(validator));
            this.undeletedRecordsCount = 0;
        }

        /// <summary>Gets the validator of this <see cref="FileCabinetFilesystemService"/> object.</summary>
        /// <value>The validator.</value>
        public IRecordValidator Validator { get; }

        /// <summary>Gets all records count.</summary>
        /// <value>All records count.</value>
        public int AllRecordsCount => this.allRecordsCount;

        /// <summary>Gets the collection of stored identifiers.</summary>
        /// <value>Collections of identifiers strored in the file cabinet service.</value>
        public ReadOnlyCollection<int> StoredIdentifiers => new ReadOnlyCollection<int>(this.storedIdentifiers);

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

            int id = this.storedIdentifiers.Count != 0 ? this.storedIdentifiers.Max() + 1 : MinValueOfId;
            var record = new FileCabinetRecord
            {
                Id = id,
                Name = new FullName(userInputData.FirstName, userInputData.LastName),
                DateOfBirth = userInputData.DateOfBirth,
                Sex = userInputData.Sex,
                NumberOfReviews = userInputData.NumberOfReviews,
                Salary = userInputData.Salary,
            };

            this.AddRecord(record);
            this.storedIdentifiers.Add(record.Id);
            this.undeletedRecordsCount++;
            return id;
        }

        /// <summary>Edits record by identifier.</summary>
        /// <param name="id">Identifier.</param>
        /// <param name="userInputData">User input data.</param>
        /// <exception cref="ArgumentNullException">Thrown when <em>userInput </em>is null.</exception>
        /// <exception cref="ArgumentException">Thrown when identifier is invalid.</exception>
        public void EditRecord(int id, UnverifiedData userInputData)
        {
            if (!this.storedIdentifiers.Contains(id))
            {
                throw new ArgumentException($"There is no #{id} record.");
            }

            if (userInputData == null)
            {
                throw new ArgumentNullException(nameof(userInputData));
            }

            this.Validator.ValidateParameters(userInputData);
            var firstNameCharArray = new char[MaxFirstNameLength];
            var lastNameCharArray = new char[MaxLastNameLength];
            for (int i = 0; i < userInputData.FirstName.Length; i++)
            {
                firstNameCharArray[i] = userInputData.FirstName[i];
            }

            for (int i = 0; i < userInputData.LastName.Length; i++)
            {
                lastNameCharArray[i] = userInputData.LastName[i];
            }

            using var reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            using var writer = new BinaryWriter(this.fileStream, Encoding.Unicode, true);
            this.fileStream.Seek(IdOffset, SeekOrigin.Begin);
            bool isNotFound = true;
            do
            {
                if (id == reader.ReadInt32())
                {
                    var currentRecordOffset = this.fileStream.Position - sizeof(int);
                    var oldFirstNameKey = new string(reader.ReadChars(MaxFirstNameLength)).Trim(NullCharacter).ToUpperInvariant();
                    this.fileStream.Seek(-2 * MaxFirstNameLength, SeekOrigin.Current);
                    this.firstNamesOffsets[oldFirstNameKey].Remove(currentRecordOffset);
                    var newFirstNameKey = userInputData.FirstName.ToUpperInvariant();
                    if (!this.firstNamesOffsets.ContainsKey(newFirstNameKey))
                    {
                        this.firstNamesOffsets.Add(newFirstNameKey, new List<long>());
                    }

                    this.firstNamesOffsets[newFirstNameKey].Add(currentRecordOffset);
                    writer.Write(firstNameCharArray);

                    var oldLastNameKey = new string(reader.ReadChars(MaxLastNameLength)).Trim(NullCharacter).ToUpperInvariant();
                    this.fileStream.Seek(-2 * MaxLastNameLength, SeekOrigin.Current);
                    this.lastNamesOffsets[oldLastNameKey].Remove(currentRecordOffset);
                    var newLastNameKey = userInputData.LastName.ToUpperInvariant();
                    if (!this.lastNamesOffsets.ContainsKey(newLastNameKey))
                    {
                        this.lastNamesOffsets.Add(newLastNameKey, new List<long>());
                    }

                    this.lastNamesOffsets[newLastNameKey].Add(currentRecordOffset);
                    writer.Write(lastNameCharArray);

                    var oldDateOfBirthKey = new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
                    this.fileStream.Seek(-3 * sizeof(int), SeekOrigin.Current);
                    this.dateOfBirthOffsets[oldDateOfBirthKey].Remove(currentRecordOffset);
                    if (!this.dateOfBirthOffsets.ContainsKey(userInputData.DateOfBirth))
                    {
                        this.dateOfBirthOffsets.Add(userInputData.DateOfBirth, new List<long>());
                    }

                    this.dateOfBirthOffsets[userInputData.DateOfBirth].Add(currentRecordOffset);
                    writer.Write(userInputData.DateOfBirth.Year);
                    writer.Write(userInputData.DateOfBirth.Month);
                    writer.Write(userInputData.DateOfBirth.Day);
                    writer.Write(userInputData.Sex);
                    writer.Write(userInputData.NumberOfReviews);
                    writer.Write(userInputData.Salary);

                    isNotFound = false;
                }
                else
                {
                    this.fileStream.Seek(RecordLenghtInBytes - sizeof(int), SeekOrigin.Current);
                }
            }
            while (isNotFound);
        }

        /// <summary>Finds records by first name.</summary>
        /// <param name="firstName">First name to find.</param>
        /// <returns>Returns a read-only collection of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (firstName == null)
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            var firstNameKey = firstName.ToUpperInvariant();
            if (this.firstNamesOffsets.ContainsKey(firstNameKey) && this.firstNamesOffsets[firstNameKey].Count != 0)
            {
                using var reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
                foreach (var offset in this.firstNamesOffsets[firstNameKey])
                {
                    this.fileStream.Seek(offset, SeekOrigin.Begin);
                    yield return new FileCabinetRecord
                    {
                        Id = reader.ReadInt32(),
                        Name = new FullName(new string(reader.ReadChars(MaxFirstNameLength)).Trim(NullCharacter), new string(reader.ReadChars(MaxLastNameLength)).Trim(NullCharacter)),
                        DateOfBirth = new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32()),
                        Sex = reader.ReadChar(),
                        NumberOfReviews = reader.ReadInt16(),
                        Salary = reader.ReadDecimal(),
                    };
                }
            }
        }

        /// <summary>Finds records by last name.</summary>
        /// <param name="lastName">Last name to find.</param>
        /// <returns>Returns a read-only collection of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (lastName == null)
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            var lastNameKey = lastName.ToUpperInvariant();
            if (this.lastNamesOffsets.ContainsKey(lastNameKey) && this.lastNamesOffsets[lastNameKey].Count != 0)
            {
                using var reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
                foreach (var offset in this.lastNamesOffsets[lastNameKey])
                {
                    this.fileStream.Seek(offset, SeekOrigin.Begin);
                    yield return new FileCabinetRecord
                    {
                        Id = reader.ReadInt32(),
                        Name = new FullName(new string(reader.ReadChars(MaxFirstNameLength)).Trim(NullCharacter), new string(reader.ReadChars(MaxLastNameLength)).Trim(NullCharacter)),
                        DateOfBirth = new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32()),
                        Sex = reader.ReadChar(),
                        NumberOfReviews = reader.ReadInt16(),
                        Salary = reader.ReadDecimal(),
                    };
                }
            }
        }

        /// <summary>Finds records by date of birth.</summary>
        /// <param name="dateOfBirth">Date of birth to find.</param>
        /// <returns>Returns a read-only collection of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            if (this.dateOfBirthOffsets.ContainsKey(dateOfBirth) && this.dateOfBirthOffsets[dateOfBirth].Count != 0)
            {
                using var reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
                foreach (var offset in this.dateOfBirthOffsets[dateOfBirth])
                {
                    this.fileStream.Seek(offset, SeekOrigin.Begin);
                    yield return new FileCabinetRecord
                    {
                        Id = reader.ReadInt32(),
                        Name = new FullName(new string(reader.ReadChars(MaxFirstNameLength)).Trim(NullCharacter), new string(reader.ReadChars(MaxLastNameLength)).Trim(NullCharacter)),
                        DateOfBirth = new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32()),
                        Sex = reader.ReadChar(),
                        NumberOfReviews = reader.ReadInt16(),
                        Salary = reader.ReadDecimal(),
                    };
                }
            }
        }

        /// <summary>Gets the records.</summary>
        /// <returns>Returns a read-only collection  of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.fileStream.Seek(BeginOfFile, SeekOrigin.Begin);
            using var reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            var records = new List<FileCabinetRecord>();
            short reservedField;
            while (reader.PeekChar() > -1)
            {
                reservedField = reader.ReadInt16();
                if ((reservedField & DeletedBitMask) == DeletedBitMask)
                {
                    this.fileStream.Seek(RecordLenghtInBytes - sizeof(short), SeekOrigin.Current);
                    continue;
                }

                var record = new FileCabinetRecord
                {
                    Id = reader.ReadInt32(),
                    Name = new FullName(new string(reader.ReadChars(MaxFirstNameLength)).Trim(NullCharacter), new string(reader.ReadChars(MaxLastNameLength)).Trim(NullCharacter)),
                    DateOfBirth = new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32()),
                    Sex = reader.ReadChar(),
                    NumberOfReviews = reader.ReadInt16(),
                    Salary = reader.ReadDecimal(),
                };

                records.Add(record);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>Gets the stat of records in the file cabinet.</summary>
        /// <returns>Returns number of records.</returns>
        public int GetStat()
        {
            this.allRecordsCount = (int)this.fileStream.Length / RecordLenghtInBytes;
            return this.undeletedRecordsCount;
        }

        /// <summary>Makes snapshot of current <see cref="FileCabinetFilesystemService"/> object state.</summary>
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
                if (this.storedIdentifiers.Contains(record.Id))
                {
                    data = new UnverifiedData(record);
                    this.EditRecord(record.Id, data);
                }
                else
                {
                    this.AddRecord(record);
                    this.storedIdentifiers.Add(record.Id);
                    this.undeletedRecordsCount++;
                }
            }
        }

        /// <summary>Removes the record from <see cref="FileCabinetFilesystemService"/> object.</summary>
        /// <param name="id">Identifier of the record to delete.</param>
        /// <exception cref="ArgumentException">Thrown when record is not found.</exception>
        public void RemoveRecord(int id)
        {
            if (!this.storedIdentifiers.Contains(id))
            {
                throw new ArgumentException($"There is no #{id} record.");
            }

            this.fileStream.Seek(IdOffset, SeekOrigin.Begin);
            using var reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            using var writer = new BinaryWriter(this.fileStream, Encoding.Unicode, true);
            bool isNotFound = true;
            do
            {
                if (id == reader.ReadInt32())
                {
                    var currentRecordOffset = this.fileStream.Position - sizeof(int);
                    this.fileStream.Seek(-FirstNameOffset, SeekOrigin.Current);
                    var reservedField = reader.ReadInt16();
                    reservedField |= DeletedBitMask;
                    this.fileStream.Seek(-sizeof(short), SeekOrigin.Current);
                    writer.Write(reservedField);
                    this.fileStream.Seek(FirstNameOffset - sizeof(short), SeekOrigin.Current);

                    var firstNameKey = new string(reader.ReadChars(MaxFirstNameLength)).Trim(NullCharacter).ToUpperInvariant();
                    this.firstNamesOffsets[firstNameKey].Remove(currentRecordOffset);

                    var lastNameKey = new string(reader.ReadChars(MaxLastNameLength)).Trim(NullCharacter).ToUpperInvariant();
                    this.lastNamesOffsets[lastNameKey].Remove(currentRecordOffset);

                    var dateOfBirthKey = new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
                    this.dateOfBirthOffsets[dateOfBirthKey].Remove(currentRecordOffset);

                    this.storedIdentifiers.Remove(id);
                    this.undeletedRecordsCount--;
                    isNotFound = false;
                }
                else
                {
                    this.fileStream.Seek(RecordLenghtInBytes - sizeof(int), SeekOrigin.Current);
                }
            }
            while (isNotFound);
        }

        /// <summary>Defragments the data file.</summary>
        /// <exception cref="InvalidOperationException">Thrown when there are no deleted records in the data file.</exception>
        public void Purge()
        {
            string filePath = this.fileStream.Name;
            if (this.GetStat() == this.allRecordsCount)
            {
                throw new InvalidOperationException("There are no deleted records.");
            }

            var records = this.GetRecords();
            this.fileStream.Dispose();
            this.fileStream = File.Create(filePath);
            foreach (var record in records)
            {
                this.AddRecord(record);
            }
        }

        private void AddRecord(FileCabinetRecord record)
        {
            using var writer = new BinaryWriter(this.fileStream, Encoding.Unicode, true);
            var firstNameCharArray = new char[MaxFirstNameLength];
            var lastNameCharArray = new char[MaxLastNameLength];
            for (int i = 0; i < record.Name.FirstName.Length; i++)
            {
                firstNameCharArray[i] = record.Name.FirstName[i];
            }

            for (int i = 0; i < record.Name.LastName.Length; i++)
            {
                lastNameCharArray[i] = record.Name.LastName[i];
            }

            var currentRecordOffset = this.fileStream.Seek(IdOffset, SeekOrigin.End);
            writer.Write(record.Id);
            var firstNameKey = record.Name.FirstName.ToUpperInvariant();
            if (!this.firstNamesOffsets.ContainsKey(firstNameKey))
            {
                this.firstNamesOffsets.Add(firstNameKey, new List<long>());
            }

            this.firstNamesOffsets[firstNameKey].Add(currentRecordOffset);

            writer.Write(firstNameCharArray);

            var lastNameKey = record.Name.LastName.ToUpperInvariant();
            if (!this.lastNamesOffsets.ContainsKey(lastNameKey))
            {
                this.lastNamesOffsets.Add(lastNameKey, new List<long>());
            }

            this.lastNamesOffsets[lastNameKey].Add(currentRecordOffset);

            writer.Write(lastNameCharArray);

            if (!this.dateOfBirthOffsets.ContainsKey(record.DateOfBirth))
            {
                this.dateOfBirthOffsets.Add(record.DateOfBirth, new List<long>());
            }

            this.dateOfBirthOffsets[record.DateOfBirth].Add(currentRecordOffset);

            writer.Write(record.DateOfBirth.Year);
            writer.Write(record.DateOfBirth.Month);
            writer.Write(record.DateOfBirth.Day);
            writer.Write(record.Sex);
            writer.Write(record.NumberOfReviews);
            writer.Write(record.Salary);
        }
    }
}

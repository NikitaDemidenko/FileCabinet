using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using static FileCabinetApp.Constants;

namespace FileCabinetApp
{
    /// <summary>Provides methods for interaction with records in file system.</summary>
    /// <seealso cref="IFileCabinetService" />
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private readonly List<int> storedIdentifiers = new List<int>();
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
            var firstNameCharArray = new char[MaxNumberOfSymbols];
            var lastNameCharArray = new char[MaxNumberOfSymbols];
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
                    writer.Write(firstNameCharArray);
                    writer.Write(lastNameCharArray);
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
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (firstName == null)
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            this.fileStream.Seek(BeginOfFile, SeekOrigin.Begin);
            using var reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            var searchResult = new List<FileCabinetRecord>();
            short reservedField;
            while (reader.PeekChar() > -1)
            {
                reservedField = reader.ReadInt16();
                if ((reservedField & DeletedBitMask) == DeletedBitMask)
                {
                    this.fileStream.Seek(RecordLenghtInBytes - sizeof(short), SeekOrigin.Current);
                    continue;
                }

                this.fileStream.Seek(FirstNameOffset - sizeof(short), SeekOrigin.Current);
                if (new string(reader.ReadChars(MaxNumberOfSymbols)).Trim(NullCharacter).Equals(firstName, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.fileStream.Seek((-2 * MaxNumberOfSymbols) - sizeof(int), SeekOrigin.Current);
                    var record = new FileCabinetRecord
                    {
                        Id = reader.ReadInt32(),
                        Name = new FullName(new string(reader.ReadChars(MaxNumberOfSymbols)).Trim(NullCharacter), new string(reader.ReadChars(MaxNumberOfSymbols)).Trim(NullCharacter)),
                        DateOfBirth = new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32()),
                        Sex = reader.ReadChar(),
                        NumberOfReviews = reader.ReadInt16(),
                        Salary = reader.ReadDecimal(),
                    };

                    searchResult.Add(record);
                }
                else
                {
                    this.fileStream.Seek(RecordLenghtInBytes - (2 * MaxNumberOfSymbols) - FirstNameOffset, SeekOrigin.Current);
                }
            }

            return searchResult.Count != 0 ? new ReadOnlyCollection<FileCabinetRecord>(searchResult) : null;
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

            this.fileStream.Seek(BeginOfFile, SeekOrigin.Begin);
            using var reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            var searchResult = new List<FileCabinetRecord>();
            short reservedField;
            while (reader.PeekChar() > -1)
            {
                reservedField = reader.ReadInt16();
                if ((reservedField & DeletedBitMask) == DeletedBitMask)
                {
                    this.fileStream.Seek(RecordLenghtInBytes - sizeof(short), SeekOrigin.Current);
                    continue;
                }

                this.fileStream.Seek(LastNameOffset - sizeof(short), SeekOrigin.Current);
                if (new string(reader.ReadChars(MaxNumberOfSymbols)).Trim(NullCharacter).Equals(lastName, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.fileStream.Seek((-4 * MaxNumberOfSymbols) - sizeof(int), SeekOrigin.Current);
                    var record = new FileCabinetRecord
                    {
                        Id = reader.ReadInt32(),
                        Name = new FullName(new string(reader.ReadChars(MaxNumberOfSymbols)).Trim(NullCharacter), new string(reader.ReadChars(MaxNumberOfSymbols)).Trim(NullCharacter)),
                        DateOfBirth = new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32()),
                        Sex = reader.ReadChar(),
                        NumberOfReviews = reader.ReadInt16(),
                        Salary = reader.ReadDecimal(),
                    };

                    searchResult.Add(record);
                }
                else
                {
                    this.fileStream.Seek(RecordLenghtInBytes - (4 * MaxNumberOfSymbols) - FirstNameOffset, SeekOrigin.Current);
                }
            }

            return searchResult.Count != 0 ? new ReadOnlyCollection<FileCabinetRecord>(searchResult) : null;
        }

        /// <summary>Finds records by date of birth.</summary>
        /// <param name="dateOfBirth">Date of birth to find.</param>
        /// <returns>Returns a read-only collection of found records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            this.fileStream.Seek(BeginOfFile, SeekOrigin.Begin);
            using var reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            var searchResult = new List<FileCabinetRecord>();
            short reservedField;
            while (reader.PeekChar() > -1)
            {
                reservedField = reader.ReadInt16();
                if ((reservedField & DeletedBitMask) == DeletedBitMask)
                {
                    this.fileStream.Seek(RecordLenghtInBytes - sizeof(short), SeekOrigin.Current);
                    continue;
                }

                this.fileStream.Seek(DateOfBirthOffset - sizeof(short), SeekOrigin.Current);
                if (reader.ReadInt32() == dateOfBirth.Year & reader.ReadInt32() == dateOfBirth.Month & reader.ReadInt32() == dateOfBirth.Day)
                {
                    this.fileStream.Seek((-4 * MaxNumberOfSymbols) - (4 * sizeof(int)), SeekOrigin.Current);
                    var record = new FileCabinetRecord
                    {
                        Id = reader.ReadInt32(),
                        Name = new FullName(new string(reader.ReadChars(MaxNumberOfSymbols)).Trim(NullCharacter), new string(reader.ReadChars(MaxNumberOfSymbols)).Trim(NullCharacter)),
                        DateOfBirth = new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32()),
                        Sex = reader.ReadChar(),
                        NumberOfReviews = reader.ReadInt16(),
                        Salary = reader.ReadDecimal(),
                    };

                    searchResult.Add(record);
                }
                else
                {
                    this.fileStream.Seek(RecordLenghtInBytes - (3 * sizeof(int)) - DateOfBirthOffset, SeekOrigin.Current);
                }
            }

            return searchResult.Count != 0 ? new ReadOnlyCollection<FileCabinetRecord>(searchResult) : null;
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
                    Name = new FullName(new string(reader.ReadChars(MaxNumberOfSymbols)).Trim(NullCharacter), new string(reader.ReadChars(MaxNumberOfSymbols)).Trim(NullCharacter)),
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
        public int GetStat() => this.undeletedRecordsCount;

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
                    this.fileStream.Seek(-FirstNameOffset, SeekOrigin.Current);
                    var reservedField = reader.ReadInt16();
                    reservedField |= DeletedBitMask;
                    this.fileStream.Seek(-sizeof(short), SeekOrigin.Current);
                    writer.Write(reservedField);
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
            this.allRecordsCount = (int)this.fileStream.Length / RecordLenghtInBytes;
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
            var firstNameCharArray = new char[MaxNumberOfSymbols];
            var lastNameCharArray = new char[MaxNumberOfSymbols];
            for (int i = 0; i < record.Name.FirstName.Length; i++)
            {
                firstNameCharArray[i] = record.Name.FirstName[i];
            }

            for (int i = 0; i < record.Name.LastName.Length; i++)
            {
                lastNameCharArray[i] = record.Name.LastName[i];
            }

            this.fileStream.Seek(IdOffset, SeekOrigin.End);
            writer.Write(record.Id);
            writer.Write(firstNameCharArray);
            writer.Write(lastNameCharArray);
            writer.Write(record.DateOfBirth.Year);
            writer.Write(record.DateOfBirth.Month);
            writer.Write(record.DateOfBirth.Day);
            writer.Write(record.Sex);
            writer.Write(record.NumberOfReviews);
            writer.Write(record.Salary);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using static FileCabinetApp.Constants;

namespace FileCabinetApp
{
    /// <summary>Provides methods for interaction with records in file system.</summary>
    /// <seealso cref="IFileCabinetService" />
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private readonly FileStream fileStream;
        private readonly IRecordValidator validator;
        private int recordsCount;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.</summary>
        /// <param name="fileStream">File stream.</param>
        /// <param name="validator">Validator.</param>
        /// <exception cref="ArgumentNullException">Thrown when fileStream or validator is null.</exception>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream ?? throw new ArgumentNullException(nameof(fileStream));
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
            this.recordsCount = 0;
        }

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

            using var writer = new BinaryWriter(this.fileStream, Encoding.Unicode, true);
            this.validator.ValidateParameters(userInputData);
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

            this.fileStream.Seek(IdOffset, SeekOrigin.End);
            writer.Write(++this.recordsCount);
            writer.Write(firstNameCharArray);
            writer.Write(lastNameCharArray);
            writer.Write(userInputData.DateOfBirth.Year);
            writer.Write(userInputData.DateOfBirth.Month);
            writer.Write(userInputData.DateOfBirth.Day);
            writer.Write(userInputData.Sex);
            writer.Write(userInputData.NumberOfReviews);
            writer.Write(userInputData.Salary);

            return this.recordsCount;
        }

        /// <summary>Edits record by identifier.</summary>
        /// <param name="id">Identifier.</param>
        /// <param name="userInputData">User input data.</param>
        /// <exception cref="ArgumentNullException">Thrown when <em>userInput </em>is null.</exception>
        /// <exception cref="ArgumentException">Thrown when identifier is invalid.</exception>
        public void EditRecord(int id, UnverifiedData userInputData)
        {
            if (id < MinValueOfId || id > this.recordsCount)
            {
                throw new ArgumentException($"There is no #{id} record.");
            }

            if (userInputData == null)
            {
                throw new ArgumentNullException(nameof(userInputData));
            }

            this.validator.ValidateParameters(userInputData);
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

                    this.fileStream.Seek(0, SeekOrigin.End);
                    isNotFound = false;
                }

                this.fileStream.Seek(RecordLenghtInBytes - sizeof(int), SeekOrigin.Current);
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

            this.fileStream.Seek(FirstNameOffset, SeekOrigin.Begin);
            using var reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            var searchResult = new List<FileCabinetRecord>();
            while (reader.PeekChar() > -1)
            {
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
                    this.fileStream.Seek(FirstNameOffset, SeekOrigin.Current);
                }
                else
                {
                    this.fileStream.Seek(RecordLenghtInBytes - (2 * MaxNumberOfSymbols), SeekOrigin.Current);
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

            this.fileStream.Seek(LastNameOffset, SeekOrigin.Begin);
            using var reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            var searchResult = new List<FileCabinetRecord>();
            while (reader.PeekChar() > -1)
            {
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
                    this.fileStream.Seek(LastNameOffset, SeekOrigin.Current);
                }
                else
                {
                    this.fileStream.Seek(RecordLenghtInBytes - (2 * MaxNumberOfSymbols), SeekOrigin.Current);
                }
            }

            return searchResult.Count != 0 ? new ReadOnlyCollection<FileCabinetRecord>(searchResult) : null;
        }

        /// <summary>Finds records by date of birth.</summary>
        /// <param name="dateOfBirth">Date of birth to find.</param>
        /// <returns>Returns a read-only collection of found records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            this.fileStream.Seek(DateOfBirthOffset, SeekOrigin.Begin);
            using var reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            var searchResult = new List<FileCabinetRecord>();
            while (reader.PeekChar() > -1)
            {
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
                    this.fileStream.Seek(DateOfBirthOffset, SeekOrigin.Current);
                }
                else
                {
                    this.fileStream.Seek(RecordLenghtInBytes - (3 * sizeof(int)), SeekOrigin.Current);
                }
            }

            return searchResult.Count != 0 ? new ReadOnlyCollection<FileCabinetRecord>(searchResult) : null;
        }

        /// <summary>Gets the records.</summary>
        /// <returns>Returns a read-only collection  of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.fileStream.Seek(IdOffset, SeekOrigin.Begin);
            using var reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            var records = new List<FileCabinetRecord>();
            while (reader.PeekChar() > -1)
            {
                var record = new FileCabinetRecord
                {
                    Id = reader.ReadInt32(),
                    Name = new FullName(new string(reader.ReadChars(MaxNumberOfSymbols)).Trim(NullCharacter), new string(reader.ReadChars(MaxNumberOfSymbols)).Trim(NullCharacter)),
                    DateOfBirth = new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32()),
                    Sex = reader.ReadChar(),
                    NumberOfReviews = reader.ReadInt16(),
                    Salary = reader.ReadDecimal(),
                };

                this.fileStream.Seek(IdOffset, SeekOrigin.Current);
                records.Add(record);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>Gets the stat of records in the file cabinet.</summary>
        /// <returns>Returns number of records.</returns>
        public int GetStat() => this.recordsCount;

        /// <summary>Makes snapshot of current <see cref="FileCabinetFilesystemService"/> object state.</summary>
        /// <returns>Returns new <see cref="FileCabinetServiceSnapshot"/>.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot() => new FileCabinetServiceSnapshot(this.GetRecords());
    }
}

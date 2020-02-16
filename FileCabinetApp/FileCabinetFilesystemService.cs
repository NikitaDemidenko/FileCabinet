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
    /// <seealso cref="FileCabinetApp.IFileCabinetService" />
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
        public int CreateRecord(UserInputData userInputData)
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

            this.fileStream.Seek(2, SeekOrigin.End);
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
        public void EditRecord(int id, UserInputData userInputData)
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
            this.fileStream.Seek(2, SeekOrigin.Begin);
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
                    break;
                }

                this.fileStream.Seek(RecordLenghtInBytes - sizeof(int), SeekOrigin.Current);
            }
            while (true);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        /// <summary>Gets the records.</summary>
        /// <returns>Returns a read-only collection  of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            using var reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            this.fileStream.Seek(2, SeekOrigin.Begin);
            var records = new List<FileCabinetRecord>();
            while (reader.PeekChar() > -1)
            {
                var record = new FileCabinetRecord
                {
                    Id = reader.ReadInt32(),
                    FirstName = new string(reader.ReadChars(MaxNumberOfSymbols)).Trim('\0'),
                    LastName = new string(reader.ReadChars(MaxNumberOfSymbols)).Trim('\0'),
                    DateOfBirth = new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32()),
                    Sex = reader.ReadChar(),
                    NumberOfReviews = reader.ReadInt16(),
                    Salary = reader.ReadDecimal(),
                };

                this.fileStream.Seek(2, SeekOrigin.Current);
                records.Add(record);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>Gets the stat of records in the file cabinet.</summary>
        /// <returns>Returns number of records.</returns>
        public int GetStat() => this.recordsCount;
    }
}

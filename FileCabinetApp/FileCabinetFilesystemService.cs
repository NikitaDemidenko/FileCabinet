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
            writer.Write(++this.recordsCount);
            writer.Write(userInputData.FirstName);
            writer.Write(userInputData.LastName);
            writer.Write(userInputData.DateOfBirth.ToString(InputDateFormat, CultureInfo.InvariantCulture));
            writer.Write(userInputData.Sex);
            writer.Write(userInputData.NumberOfReviews);
            writer.Write(userInputData.Salary);

            return this.recordsCount;
        }

        public void EditRecord(int id, UserInputData userInputData)
        {
            throw new NotImplementedException();
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
            this.fileStream.Seek(0, SeekOrigin.Begin);
            var records = new List<FileCabinetRecord>();
            while (reader.PeekChar() > -1)
            {
                var record = new FileCabinetRecord
                {
                    Id = reader.ReadInt32(),
                    FirstName = reader.ReadString(),
                    LastName = reader.ReadString(),
                    DateOfBirth = DateTime.Parse(reader.ReadString(), CultureInfo.InvariantCulture),
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
        public int GetStat() => this.recordsCount;
    }
}

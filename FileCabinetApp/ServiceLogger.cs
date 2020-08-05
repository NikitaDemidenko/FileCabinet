using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using static FileCabinetApp.ConstantsAndValidationRulesSettings.Constants;

namespace FileCabinetApp
{
    /// <summary>Service logger.</summary>
    /// <seealso cref="IFileCabinetService" />
    public class ServiceLogger : IFileCabinetService
    {
        private readonly IFileCabinetService service;

        /// <summary>Initializes a new instance of the <see cref="ServiceLogger"/> class.</summary>
        /// <param name="service">The service.</param>
        /// <exception cref="ArgumentNullException">Thrown when service
        /// is null.</exception>
        public ServiceLogger(IFileCabinetService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>Gets the validator of this <see cref="IFileCabinetService"/> object.</summary>
        /// <value>The validator.</value>
        public IRecordValidator Validator => this.service.Validator;

        /// <summary>Gets the collection of stored identifiers.</summary>
        /// <value>Collections of identifiers strored in the file cabinet service.</value>
        public ReadOnlyCollection<int> StoredIdentifiers => this.service.StoredIdentifiers;

        /// <summary>Gets all records count.</summary>
        /// <value>All records count.</value>
        public int AllRecordsCount => this.service.AllRecordsCount;

        /// <summary>Creates new <see cref="FileCabinetRecord"/> instance.</summary>
        /// <param name="unverifiedData">Raw data.</param>
        /// <returns>Returns identifier of the new <see cref="FileCabinetRecord"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when unverifiedData
        /// is null.</exception>
        public int CreateRecord(UnverifiedData unverifiedData)
        {
            using var writer = new StreamWriter(LogFileName, true, Encoding.UTF8);
            if (unverifiedData == null)
            {
                writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - CreateRecord() threw an exception: {nameof(unverifiedData)} is null");
                throw new ArgumentNullException(nameof(unverifiedData));
            }

            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - Calling CreateRecord() with FirstName = '{unverifiedData.FirstName}', " +
                    $"LastName = '{unverifiedData.LastName}', DateOfBirth = '{unverifiedData.DateOfBirth.ToString(InputDateFormat, Culture)}', " +
                    $"Sex = '{unverifiedData.Sex}', NumberOfReviews = '{unverifiedData.NumberOfReviews}', Salary = '{unverifiedData.Salary}'");
            try
            {
                var result = this.service.CreateRecord(unverifiedData);
                writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - CreateRecord() returned '{result}'");
                return result;
            }
            catch (ArgumentException ex)
            {
                writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - CreateRecord() threw an exception: {ex.Message}");
                throw;
            }
        }

        /// <summary>Edits record by identifier.</summary>
        /// <param name="id">Identifier.</param>
        /// <param name="unverifiedData">User input data.</param>
        /// <exception cref="ArgumentNullException">Thrown when unverifiedData
        /// is null.</exception>
        public void EditRecord(int id, UnverifiedData unverifiedData)
        {
            using var writer = new StreamWriter(LogFileName, true, Encoding.UTF8);
            if (unverifiedData == null)
            {
                writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - EditRecord() threw an exception: {nameof(unverifiedData)} is null");
                throw new ArgumentNullException(nameof(unverifiedData));
            }

            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - Calling EditRecord() with Id = '{id}', FirstName = '{unverifiedData.FirstName}', " +
                    $"LastName = '{unverifiedData.LastName}', DateOfBirth = '{unverifiedData.DateOfBirth.ToString(InputDateFormat, Culture)}', " +
                    $"Sex = '{unverifiedData.Sex}', NumberOfReviews = '{unverifiedData.NumberOfReviews}', Salary = '{unverifiedData.Salary}'");
            try
            {
                this.service.EditRecord(id, unverifiedData);
                writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - EditRecord() executed successfully");
            }
            catch (ArgumentException ex)
            {
                writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - EditRecord() threw an exception: {ex.Message}");
                throw;
            }
        }

        /// <summary>Finds records by date of birth.</summary>
        /// <param name="dateOfBirth">Date of birth to find.</param>
        /// <returns>Returns a read-only collection of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            using var writer = new StreamWriter(LogFileName, true, Encoding.UTF8);
            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - Calling FindByDateOfBirth() with dateOfBirth = '{dateOfBirth.ToString(InputDateFormat, Culture)}'");
            var result = this.service.FindByDateOfBirth(dateOfBirth);
            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - FindByDateOfBirth() returned {result.Count()} record(s)");
            return result;
        }

        /// <summary>Finds records by first name.</summary>
        /// <param name="firstName">First name to find.</param>
        /// <returns>Returns a read-only collection of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            using var writer = new StreamWriter(LogFileName, true, Encoding.UTF8);
            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - Calling FindByFirstName() with firstName = '{firstName}'");
            var result = this.service.FindByFirstName(firstName);
            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - FindByFirstName() returned {result.Count()} record(s)");
            return result;
        }

        /// <summary>Finds records by last name.</summary>
        /// <param name="lastName">Last name to find.</param>
        /// <returns>Returns a read-only collection of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            using var writer = new StreamWriter(LogFileName, true, Encoding.UTF8);
            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - Calling FindByLastName() with lastName = '{lastName}'");
            var result = this.service.FindByLastName(lastName);
            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - FindByLastName() returned {result.Count()} record(s)");
            return result;
        }

        /// <summary>Gets the records.</summary>
        /// <returns>Returns a read-only collection  of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            using var writer = new StreamWriter(LogFileName, true, Encoding.UTF8);
            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - Calling GetRecords()");
            var result = this.service.GetRecords();
            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - GetRecords() returned {result.Count} record(s)");
            return result;
        }

        /// <summary>Gets the stat of records in the file cabinet.</summary>
        /// <returns>Returns number of records.</returns>
        public int GetStat()
        {
            using var writer = new StreamWriter(LogFileName, true, Encoding.UTF8);
            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - Calling GetStat()");
            var result = this.service.GetStat();
            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - GetStat() returned '{result}'");
            return result;
        }

        /// <summary>Makes snapshot of current object state.</summary>
        /// <returns>Returns new <see cref="FileCabinetServiceSnapshot"/>.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            using var writer = new StreamWriter(LogFileName, true, Encoding.UTF8);
            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - Calling MakeSnapshot()");
            var result = this.service.MakeSnapshot();
            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - MakeSnapshot() returned new '{result.GetType().Name}' object");
            return result;
        }

        /// <summary>Removes the record from <see cref="IFileCabinetService"/> object.</summary>
        /// <param name="id">Identifier of the record to delete.</param>
        public void RemoveRecord(int id)
        {
            using var writer = new StreamWriter(LogFileName, true, Encoding.UTF8);
            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - Calling RemoveRecord() with Id = '{id}'");
            try
            {
                this.service.RemoveRecord(id);
                writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - RemoveRecord() executed successfully");
            }
            catch (ArgumentException ex)
            {
                writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - RemoveRecord() threw an exception: {ex.Message}");
                throw;
            }
        }

        /// <summary>Restores the specified snapshot.</summary>
        /// <param name="snapshot">Snapshot.</param>
        /// <exception cref="ArgumentNullException">Thrown when snapshot
        /// is null.</exception>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            using var writer = new StreamWriter(LogFileName, true, Encoding.UTF8);
            if (snapshot == null)
            {
                writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - Restore() threw an exception: {nameof(snapshot)} is null");
                throw new ArgumentNullException(nameof(snapshot));
            }

            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - Calling Restore() with snapshot with {snapshot.Records.Count} record(s)");
            this.service.Restore(snapshot);
            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - Restore() executed successfully");
        }

        /// <summary>Defragments the data file.</summary>
        public void Purge()
        {
            using var writer = new StreamWriter(LogFileName, true, Encoding.UTF8);
            writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - Calling Purge()");
            try
            {
                this.service.Purge();
                writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - Purge() executed successfully");
            }
            catch (NotSupportedException)
            {
                writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - Purge() threw an exception: This method works with file system only.");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                writer.WriteLine($"{DateTime.Now.ToString(LogDateFormat, Culture)} - Purge() threw an exception: {ex.Message}");
                throw;
            }
        }
    }
}

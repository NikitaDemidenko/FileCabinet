using System;
using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>Provides functionality for interaction with records in the file cabinet.</summary>
    public interface IFileCabinetService
    {
        /// <summary>Gets the validator of this <see cref="IFileCabinetService"/> object.</summary>
        /// <value>The validator.</value>
        public IRecordValidator Validator { get; }

        /// <summary>Gets the collection of stored identifiers.</summary>
        /// <value>Collections of identifiers strored in the file cabinet service.</value>
        public ReadOnlyCollection<int> StoredIdentifiers { get; }

        /// <summary>Gets all records count.</summary>
        /// <value>All records count.</value>
        public int AllRecordsCount { get; }

        /// <summary>Creates new <see cref="FileCabinetRecord"/> instance.</summary>
        /// <param name="unverifiedData">Raw data.</param>
        /// <returns>Returns identifier of the new <see cref="FileCabinetRecord"/> instance.</returns>
        public int CreateRecord(UnverifiedData unverifiedData);

        /// <summary>Gets the records.</summary>
        /// <returns>Returns a read-only collection  of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>Gets the stat of records in the file cabinet.</summary>
        /// <returns>Returns number of records.</returns>
        public int GetStat();

        /// <summary>Edits record by identifier.</summary>
        /// <param name="id">Identifier.</param>
        /// <param name="unverifiedData">User input data.</param>
        public void EditRecord(int id, UnverifiedData unverifiedData);

        /// <summary>Removes the record from <see cref="IFileCabinetService"/> object.</summary>
        /// <param name="id">Identifier of the record to delete.</param>
        public void RemoveRecord(int id);

        /// <summary>Finds records by first name.</summary>
        /// <param name="firstName">First name to find.</param>
        /// <returns>Returns a read-only collection of found records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>Finds records by last name.</summary>
        /// <param name="lastName">Last name to find.</param>
        /// <returns>Returns a read-only collection of found records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>Finds records by date of birth.</summary>
        /// <param name="dateOfBirth">Date of birth to find.</param>
        /// <returns>Returns a read-only collection of found records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth);

        /// <summary>Makes snapshot of current object state.</summary>
        /// /// <returns>Returns new <see cref="FileCabinetServiceSnapshot"/>.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>Restores the specified snapshot.</summary>
        /// <param name="snapshot">Snapshot.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot);
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>Service meter.</summary>
    /// <seealso cref="IFileCabinetService" />
    public class ServiceMeter : IFileCabinetService
    {
        private readonly IFileCabinetService service;

        /// <summary>Initializes a new instance of the <see cref="ServiceMeter"/> class.</summary>
        /// <param name="service">The service.</param>
        /// <exception cref="ArgumentNullException">Thrown when service
        /// is null.</exception>
        public ServiceMeter(IFileCabinetService service)
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
        public int CreateRecord(UnverifiedData unverifiedData)
        {
            var timer = new Stopwatch();
            timer.Start();
            var result = this.service.CreateRecord(unverifiedData);
            timer.Stop();
            Console.WriteLine($"CreateRecord method execution duration is {timer.ElapsedMilliseconds} ticks.");
            return result;
        }

        /// <summary>Edits record by identifier.</summary>
        /// <param name="id">Identifier.</param>
        /// <param name="unverifiedData">User input data.</param>
        public void EditRecord(int id, UnverifiedData unverifiedData)
        {
            var timer = new Stopwatch();
            timer.Start();
            this.service.EditRecord(id, unverifiedData);
            timer.Stop();
            Console.WriteLine($"EditRecord method execution duration is {timer.ElapsedMilliseconds} ticks.");
        }

        /// <summary>Finds records by date of birth.</summary>
        /// <param name="dateOfBirth">Date of birth to find.</param>
        /// <returns>Returns a read-only collection of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            var timer = new Stopwatch();
            timer.Start();
            var result = this.service.FindByDateOfBirth(dateOfBirth);
            timer.Stop();
            Console.WriteLine($"FindByDateOfBirth method execution duration is {timer.ElapsedMilliseconds} ticks.");
            return result;
        }

        /// <summary>Finds records by first name.</summary>
        /// <param name="firstName">First name to find.</param>
        /// <returns>Returns a read-only collection of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var timer = new Stopwatch();
            timer.Start();
            var result = this.service.FindByFirstName(firstName);
            timer.Stop();
            Console.WriteLine($"FindByFirstName method execution duration is {timer.ElapsedMilliseconds} ticks.");
            return result;
        }

        /// <summary>Finds records by last name.</summary>
        /// <param name="lastName">Last name to find.</param>
        /// <returns>Returns a read-only collection of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            var timer = new Stopwatch();
            timer.Start();
            var result = this.service.FindByLastName(lastName);
            timer.Stop();
            Console.WriteLine($"FindByLastName method execution duration is {timer.ElapsedMilliseconds} ticks.");
            return result;
        }

        /// <summary>Gets the records.</summary>
        /// <returns>Returns a read-only collection  of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var timer = new Stopwatch();
            timer.Start();
            var result = this.service.GetRecords();
            timer.Stop();
            Console.WriteLine($"GetRecords method execution duration is {timer.ElapsedMilliseconds} ticks.");
            return result;
        }

        /// <summary>Gets the stat of records in the file cabinet.</summary>
        /// <returns>Returns number of records.</returns>
        public int GetStat()
        {
            var timer = new Stopwatch();
            timer.Start();
            var result = this.service.GetStat();
            timer.Stop();
            Console.WriteLine($"GetStat method execution duration is {timer.ElapsedMilliseconds} ticks.");
            return result;
        }

        /// <summary>Makes snapshot of current object state.</summary>
        /// <returns>Returns new <see cref="FileCabinetServiceSnapshot"/>.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            var timer = new Stopwatch();
            timer.Start();
            var result = this.service.MakeSnapshot();
            timer.Stop();
            Console.WriteLine($"MakeSnapshot method execution duration is {timer.ElapsedMilliseconds} ticks.");
            return result;
        }

        /// <summary>Removes the record from <see cref="IFileCabinetService"/> object.</summary>
        /// <param name="id">Identifier of the record to delete.</param>
        public void RemoveRecord(int id)
        {
            var timer = new Stopwatch();
            timer.Start();
            this.service.RemoveRecord(id);
            timer.Stop();
            Console.WriteLine($"RemoveRecord method execution duration is {timer.ElapsedMilliseconds} ticks.");
        }

        /// <summary>Restores the specified snapshot.</summary>
        /// <param name="snapshot">Snapshot.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            var timer = new Stopwatch();
            timer.Start();
            this.service.Restore(snapshot);
            timer.Stop();
            Console.WriteLine($"Restore method execution duration is {timer.ElapsedMilliseconds} ticks.");
        }

        /// <summary>Defragments the data file.</summary>
        public void Purge()
        {
            var timer = new Stopwatch();
            timer.Start();
            this.service.Purge();
            timer.Stop();
            Console.WriteLine($"Purge method execution duration is {timer.ElapsedMilliseconds} ticks.");
        }
    }
}

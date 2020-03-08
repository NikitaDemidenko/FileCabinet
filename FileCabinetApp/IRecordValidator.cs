namespace FileCabinetApp
{
    /// <summary>
    ///   <para>Provides functionality to validate parameters.</para>
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>Validates user input data.</summary>
        /// <param name="data">Data to validate.</param>
        public void ValidateParameters(UnverifiedData data);
    }
}

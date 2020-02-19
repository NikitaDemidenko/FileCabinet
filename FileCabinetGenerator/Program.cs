using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using static FileCabinetGenerator.Constants;

namespace FileCabinetGenerator
{
    /// <summary>Main class of the project.</summary>
    public static class Program
    {
        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            var flagValuePairs = ParseFlags(args);
            if (flagValuePairs == null)
            {
                Console.WriteLine("Invalid flags.");
                return;
            }

            if (int.TryParse(flagValuePairs[Flags.RecordsAmount], out int recordsAmount))
            {
                if (recordsAmount < 1)
                {
                    Console.WriteLine("Amount of records must be greater than zero.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Invalid records amount.");
                return;
            }

            if (int.TryParse(flagValuePairs[Flags.StartId], out int startId))
            {
                if (startId < 1)
                {
                    Console.WriteLine("Start ID must be greater than zero.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Invalid start ID.");
                return;
            }

            string filePath = flagValuePairs[Flags.Output];
            string formatType = flagValuePairs[Flags.OutputType];
            if (formatType.Equals(CsvFileExtension, StringComparison.InvariantCultureIgnoreCase))
            {
                SaveToCsv(filePath, recordsAmount, startId);
                Console.WriteLine($"{recordsAmount} records were written to {filePath}.");
            }
            else if (formatType.Equals(XmlFileExtension, StringComparison.InvariantCultureIgnoreCase))
            {
                SaveToXml(filePath, recordsAmount, startId);
                Console.WriteLine($"{recordsAmount} records were written to {filePath}.");
            }
            else
            {
                Console.WriteLine("Invalid file format.");
                return;
            }
        }

        private static Dictionary<Flags, string> ParseFlags(string[] args)
        {
            if (args == null || args.Length != 8)
            {
                return null;
            }

            var flagValuePairs = new Dictionary<Flags, string>();
            var argumentsStack = new Stack<string>(args);

            while (argumentsStack.Count != 0)
            {
                var arg = argumentsStack.Pop();
                switch (argumentsStack.Pop())
                {
                    case OutputTypeFullPropertyName:
                        flagValuePairs.Add(Flags.OutputType, arg);
                        break;
                    case OutputTypeShortcutPropertyName:
                        flagValuePairs.Add(Flags.OutputType, arg);
                        break;
                    case OutputFullPropertyName:
                        flagValuePairs.Add(Flags.Output, arg);
                        break;
                    case OutputShortcutPropertyName:
                        flagValuePairs.Add(Flags.Output, arg);
                        break;
                    case RecordsAmountFullPropertyName:
                        flagValuePairs.Add(Flags.RecordsAmount, arg);
                        break;
                    case RecordsAmountShortcutPropertyName:
                        flagValuePairs.Add(Flags.RecordsAmount, arg);
                        break;
                    case StartIdFullPropertyName:
                        flagValuePairs.Add(Flags.StartId, arg);
                        break;
                    case StartIdShortcutPropertyName:
                        flagValuePairs.Add(Flags.StartId, arg);
                        break;
                    default:
                        return null;
                }
            }

            return flagValuePairs;
        }

        private static void SaveToCsv(string filePath, int count, int startId)
        {
            throw new NotImplementedException();
        }

        private static void SaveToXml(string filePath, int count, int startId)
        {
            throw new NotImplementedException();
        }
    }
}

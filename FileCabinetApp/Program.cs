using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.ParametersValidators;
using static FileCabinetApp.ConstantsAndValidationRulesSettings.Constants;

namespace FileCabinetApp
{
    /// <summary>Main class of the project.</summary>
    public static class Program
    {
        private const string DeveloperName = "Nikita Demidenko";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";

        private static List<string> commandLineFlags;
        private static List<string> commandLineArguments;
        private static IFileCabinetService fileCabinetService;
        private static FileStream fileStream;

        private static bool isRunning = true;
        private static bool isCustomValidationRules = false;

        private static Action<bool> setProgramStatus =
            isRunning => Program.isRunning = isRunning;

        private static IRecordValidator defaultValidator = new ValidatorBuilder().CreateDefault();

        private static IRecordValidator customValidator = new ValidatorBuilder().CreateCustom();

        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            switch (ParseFlags(args))
            {
                case Flags.Default:
                    Console.WriteLine("Using default validation rules. Records will be stored in memory.");
                    fileCabinetService = new FileCabinetMemoryService(defaultValidator);
                    break;
                case Flags.ValidationRules:
                    if (commandLineArguments.Contains(DefaultValidationRulesName))
                    {
                        Console.WriteLine("Using default validation rules. Records will be stored in memory.");
                        fileCabinetService = new FileCabinetMemoryService(defaultValidator);
                        break;
                    }
                    else if (commandLineArguments.Contains(CustomValidationRulesName))
                    {
                        Console.WriteLine("Using custom validation rules. Records will be stored in memory.");
                        fileCabinetService = new FileCabinetMemoryService(customValidator);
                        isCustomValidationRules = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid arguments.");
                        Console.WriteLine("Using default validation rules. Records will be stored in memory.");
                        fileCabinetService = new FileCabinetMemoryService(defaultValidator);
                        break;
                    }

                case Flags.Storage:
                    if (commandLineArguments.Contains(MemoryStorageName))
                    {
                        Console.WriteLine("Using default validation rules. Records will be stored in memory.");
                        fileCabinetService = new FileCabinetMemoryService(defaultValidator);
                        break;
                    }
                    else if (commandLineArguments.Contains(FileStorageName))
                    {
                        Console.WriteLine("Using default validation rules. Records will be stored in file system.");
                        fileStream = new FileStream(DbFileName, FileMode.Create);
                        fileCabinetService = new FileCabinetFilesystemService(fileStream, defaultValidator);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid arguments.");
                        Console.WriteLine("Using default validation rules. Records will be stored in memory.");
                        fileCabinetService = new FileCabinetMemoryService(defaultValidator);
                        break;
                    }

                case Flags.ValidationRules | Flags.Storage:
                    if (commandLineArguments.Contains(DefaultValidationRulesName) && commandLineArguments.Contains(MemoryStorageName))
                    {
                        Console.WriteLine("Using default validation rules. Records will be stored in memory.");
                        fileCabinetService = new FileCabinetMemoryService(defaultValidator);
                        break;
                    }
                    else if (commandLineArguments.Contains(CustomValidationRulesName) && commandLineArguments.Contains(MemoryStorageName))
                    {
                        Console.WriteLine("Using custom validation rules. Records will be stored in memory.");
                        fileCabinetService = new FileCabinetMemoryService(customValidator);
                        isCustomValidationRules = true;
                        break;
                    }
                    else if (commandLineArguments.Contains(DefaultValidationRulesName) && commandLineArguments.Contains(FileStorageName))
                    {
                        Console.WriteLine("Using default validation rules. Records will be stored in file system.");
                        fileStream = new FileStream(DbFileName, FileMode.Create);
                        fileCabinetService = new FileCabinetFilesystemService(fileStream, defaultValidator);
                        break;
                    }
                    else if (commandLineArguments.Contains(CustomValidationRulesName) && commandLineArguments.Contains(FileStorageName))
                    {
                        Console.WriteLine("Using custom validation rules. Records will be stored in file system.");
                        fileStream = new FileStream(DbFileName, FileMode.Create);
                        fileCabinetService = new FileCabinetFilesystemService(fileStream, customValidator);
                        isCustomValidationRules = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid arguments.");
                        Console.WriteLine("Using default validation rules. Records will be stored in memory.");
                        fileCabinetService = new FileCabinetMemoryService(defaultValidator);
                        break;
                    }
            }

            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();
            var commandHandler = CreateCommandHandlers();
            do
            {
                Console.Write("> ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine(HintMessage);
                    continue;
                }

                var inputs = input.Split(SpaceSymbol, NumberOfParameters);
                const int commandIndex = 0;
                const int parametersIndex = 1;
                var command = inputs[commandIndex];
                string parameters = string.Empty;
                if (inputs.Length > 1)
                {
                    parameters = inputs[parametersIndex];
                }

                commandHandler.Handle(new AppCommandRequest(command, parameters));
            }
            while (isRunning);

            fileStream?.Dispose();
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var helpHandler = new HelpCommandHandler();
            var createHandler = new CreateCommandHandler(fileCabinetService, isCustomValidationRules);
            var editHandler = new EditCommandHandler(fileCabinetService, isCustomValidationRules);
            var exitHandler = new ExitCommandHandler(setProgramStatus);
            var exporthandler = new ExportCommandHandler(fileCabinetService);
            var findHandler = new FindCommandHandler(fileCabinetService, DefaultRecordPrint);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var listHandler = new ListCommandHandler(fileCabinetService, DefaultRecordPrint);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            var removeHandler = new RemoveCommandHandler(fileCabinetService);
            var statHandler = new StatCommandHandler(fileCabinetService);

            helpHandler.SetNext(createHandler);
            createHandler.SetNext(editHandler);
            editHandler.SetNext(exitHandler);
            exitHandler.SetNext(exporthandler);
            exporthandler.SetNext(findHandler);
            findHandler.SetNext(importHandler);
            importHandler.SetNext(listHandler);
            listHandler.SetNext(purgeHandler);
            purgeHandler.SetNext(removeHandler);
            removeHandler.SetNext(statHandler);

            return helpHandler;
        }

        private static void DefaultRecordPrint(IEnumerable<FileCabinetRecord> records)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            foreach (var record in records)
            {
                Console.WriteLine(record);
            }
        }

        private static Flags ParseFlags(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            commandLineArguments = new List<string>();
            commandLineFlags = new List<string>();
            foreach (var arg in args)
            {
                commandLineArguments.Add(arg.ToUpperInvariant());
            }

            foreach (var arg in commandLineArguments)
            {
                if (new Regex(@"^--|^-").IsMatch(arg))
                {
                    commandLineFlags.Add(arg);
                }
            }

            var flags = Flags.Default;
            foreach (var flag in commandLineFlags)
            {
                switch (flag)
                {
                    case ValidationRulesFullPropertyName:
                        flags |= Flags.ValidationRules;
                        break;
                    case ValidationRulesShortcutPropertyName:
                        flags |= Flags.ValidationRules;
                        break;
                    case StorageFullPropertyName:
                        flags |= Flags.Storage;
                        break;
                    case StorageShortcutPropertyName:
                        flags |= Flags.Storage;
                        break;
                    default:
                        break;
                }

                commandLineArguments.Remove(flag);
            }

            return flags;
        }
    }
}

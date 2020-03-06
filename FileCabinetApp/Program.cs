using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FileCabinetApp.CommandHandlers;
using static FileCabinetApp.Constants;

namespace FileCabinetApp
{
    /// <summary>Main class of the project.</summary>
    public static class Program
    {
        public static IFileCabinetService fileCabinetService;
        public static bool isRunning = true;
        public static bool isCustomValidationRules = false;

        private const string DeveloperName = "Nikita Demidenko";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";

        private static List<string> commandLineFlags;
        private static List<string> commandLineArguments;
        private static FileStream fileStream;

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
                    fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                    break;
                case Flags.ValidationRules:
                    if (commandLineArguments.Contains(DefaultValidationRulesName))
                    {
                        Console.WriteLine("Using default validation rules. Records will be stored in memory.");
                        fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                        break;
                    }
                    else if (commandLineArguments.Contains(CustomValidationRulesName))
                    {
                        Console.WriteLine("Using custom validation rules. Records will be stored in memory.");
                        fileCabinetService = new FileCabinetMemoryService(new CustomValidator());
                        isCustomValidationRules = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid arguments.");
                        Console.WriteLine("Using default validation rules. Records will be stored in memory.");
                        fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                        break;
                    }

                case Flags.Storage:
                    if (commandLineArguments.Contains(MemoryStorageName))
                    {
                        Console.WriteLine("Using default validation rules. Records will be stored in memory.");
                        fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                        break;
                    }
                    else if (commandLineArguments.Contains(FileStorageName))
                    {
                        Console.WriteLine("Using default validation rules. Records will be stored in file system.");
                        fileStream = new FileStream(DbFileName, FileMode.Create);
                        fileCabinetService = new FileCabinetFilesystemService(fileStream, new DefaultValidator());
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid arguments.");
                        Console.WriteLine("Using default validation rules. Records will be stored in memory.");
                        fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                        break;
                    }

                case Flags.ValidationRules | Flags.Storage:
                    if (commandLineArguments.Contains(DefaultValidationRulesName) && commandLineArguments.Contains(MemoryStorageName))
                    {
                        Console.WriteLine("Using default validation rules. Records will be stored in memory.");
                        fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                        break;
                    }
                    else if (commandLineArguments.Contains(CustomValidationRulesName) && commandLineArguments.Contains(MemoryStorageName))
                    {
                        Console.WriteLine("Using custom validation rules. Records will be stored in memory.");
                        fileCabinetService = new FileCabinetMemoryService(new CustomValidator());
                        isCustomValidationRules = true;
                        break;
                    }
                    else if (commandLineArguments.Contains(DefaultValidationRulesName) && commandLineArguments.Contains(FileStorageName))
                    {
                        Console.WriteLine("Using default validation rules. Records will be stored in file system.");
                        fileStream = new FileStream(DbFileName, FileMode.Create);
                        fileCabinetService = new FileCabinetFilesystemService(fileStream, new DefaultValidator());
                        break;
                    }
                    else if (commandLineArguments.Contains(CustomValidationRulesName) && commandLineArguments.Contains(FileStorageName))
                    {
                        Console.WriteLine("Using custom validation rules. Records will be stored in file system.");
                        fileStream = new FileStream(DbFileName, FileMode.Create);
                        fileCabinetService = new FileCabinetFilesystemService(fileStream, new CustomValidator());
                        isCustomValidationRules = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid arguments.");
                        Console.WriteLine("Using default validation rules. Records will be stored in memory.");
                        fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                        break;
                    }
            }

            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();
            var commandHandler = CreateCommandHandlers();
            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                const int parametersIndex = 1;
                var command = inputs[commandIndex];
                string parameters = string.Empty;
                if (inputs.Length > 1)
                {
                    parameters = inputs[parametersIndex];
                }

                var request = new AppCommandRequest(command, parameters);

                commandHandler.Handle(request);
            }
            while (isRunning);

            fileStream?.Dispose();
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var commandHandler = new CommandHandler();
            return commandHandler;
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

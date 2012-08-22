using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SQLUnitRunner.Configuration;

namespace TestHarness
{
	class Program
	{
        public static CommandLineArguments Options { get; private set; }

        private static void Main(string[] args)
        {
            try
            {
                Options = new CommandLineArguments(args);

                if (Options.HelpRequested)
                {
                    Console.WriteLine(Options.UsageMessage);
                    return;
                }

                var validationErrors = new List<string>();
                if (!Options.Validate(validationErrors))
                {
                    foreach (var error in validationErrors)
                        Console.WriteLine("Error. {0}", error);

                    Environment.Exit(-1);
                }

                Console.WriteLine("Running tests from {0}\\{1} on connection {2}",
                    Options.InputFolder, Options.FileSpec, Options.ConnectionString);

                var exitCode = 0;

                FluentTestRunner.ConfigureAndRun()
                    .FromFilesIn(Options.InputFolder)
                    .WithFileSpec(Options.FileSpec)
                    .OutputLogTo(Console.Out)
                    .WhenFinishedCall((sender, eventArgs) =>
                    {
                        if (eventArgs.TestsFailed > 0)
                            exitCode = -1;
                    })
                    .RunOn(new SqlConnection(Options.ConnectionString));

                Environment.Exit(exitCode);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
                Environment.Exit(-1);
            }
        }
	}
}

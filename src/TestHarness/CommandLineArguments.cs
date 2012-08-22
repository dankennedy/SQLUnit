using System;
using System.Collections;
using System.Collections.Generic;

namespace TestHarness
{
    internal class CommandLineArguments : CommandLineArgumentsBase
    {
        private const string ARG_HELP = "?";
        private const string ARG_CONNECTIONSTRING = "con";
        private const string ARG_FOLDER = "i";
        private const string ARG_FILESPEC = "spec";

        private static readonly string[] AllArgs = {
                                                       ARG_HELP,
                                                       ARG_CONNECTIONSTRING,
                                                       ARG_FOLDER,
                                                       ARG_FILESPEC
                                                   };

        /// <summary>
        ///   Creates a new instance from the tokenized array of command line arguments
        /// </summary>
        /// <param name = "args">Tokenized arguments</param>
        public CommandLineArguments(string[] args) : base(args)
        {
            Array.Sort(AllArgs);
        }

        /// <summary>
        ///   Creates a new instance from the command line string used to execute the application
        /// </summary>
        /// <param name = "commandLine">Full command line string including the executable path and arguments</param>
        public CommandLineArguments(string commandLine) : base(commandLine)
        {
            Array.Sort(AllArgs);
        }

        #region Argument Properties

        /// <summary>
        ///   Was help or usage options requested
        /// </summary>
        public bool HelpRequested
        {
            get { return Parameters.ContainsKey(ARG_HELP); }
        }

        /// <summary>
        ///   Connection string to run tests on
        /// </summary>
        public string ConnectionString
        {
            get { return Parameters[ARG_CONNECTIONSTRING] ?? String.Empty; }
        }

        /// <summary>
        ///   Path to where test files can be found
        /// </summary>
        public string InputFolder
        {
            get { return Parameters[ARG_FOLDER] ?? System.Reflection.Assembly.GetExecutingAssembly().Location; }
        }
        
        /// <summary>
        ///   Filter to apply for test files
        /// </summary>
        public string FileSpec
        {
            get { return Parameters[ARG_FILESPEC] ?? "*.sql"; }
        }

        /// <summary>
        ///   The message to display in the console describing how to run the program
        /// </summary>
        public override string UsageMessage
        {
            get
            {
                return string.Concat("SQLTestRunner - Command line data testing utility\r\n\r\n",
                                     "Usage: SQLTestRunner.exe\r\n",
                                     "             [-?]\r\n",
                                     "             [-con connection string]\r\n",
                                     "             [-i input folder]\r\n",
                                     "             [-spec test file pattern]\r\n",
                                     "\r\n",
                                     "Example:\r\n",
                                     "SQLTestRunner.exe -con Server=localhost;Database=master;Trusted_Connection=true -i C:\\Temp -spec *MyTest.sql\r\n");
            }
        }

        #endregion

        #region Methods

        public override bool Validate(IList validationErrors)
        {
            if (validationErrors == null)
                validationErrors = new List<string>();

            var errorCountOnEntry = validationErrors.Count;

            foreach (string param in Parameters.Keys)
            {
                if (Array.BinarySearch(AllArgs, param) < 0)
                    validationErrors.Add(String.Format("Invalid argument '{0}'", param));
            }

            if (string.IsNullOrEmpty(ConnectionString))
                validationErrors.Add(
                    "Invalid or missing database connection information.");

            return validationErrors.Count == errorCountOnEntry;
        }

        #endregion
    }
}
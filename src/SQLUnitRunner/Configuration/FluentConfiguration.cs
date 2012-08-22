using System;
using System.Data;
using System.IO;

namespace SQLUnitRunner.Configuration
{
	/// <summary>
	/// Holds the logic to allow fluent configuration
	/// and execution of a set of tests based on the 
	/// pre-existing knowledge that certain types work
	/// together.  This would need to be enhanced should the
	/// number and type of runners, testers or loaders were
	/// increased.
	/// </summary>
	public class FluentConfiguration
	{
		private ISqlTestRunner runner;
		private IDbConnection connection;

		public FluentConfiguration()
		{
			runner = new SqlTestRunner(null);
		}

		public FluentConfiguration FromFilesIn()
		{
			return FromFilesIn(Environment.CurrentDirectory);
		}

		public FluentConfiguration FromFilesIn(string folderPath)
		{
			runner = new SqlTestRunner(new FolderTestLoader(new FileTestReader(), folderPath));
			return this;
		}

		public FluentConfiguration WithFileSpec(string fileSpec)
		{
			if (runner == null)
				FromFilesIn();

			if (!(runner.TestLoader is FolderTestLoader))
				throw new InvalidOperationException("File spec configuration requires a FolderTestLoader. Try calling FromFilesIn.");

			((FolderTestLoader)runner.TestLoader).FilePattern = fileSpec;
			return this;
		}

		public FluentConfiguration OnConnection(IDbConnection con)
		{
			if (runner == null)
				FromFilesIn();

			connection = con;
			return this;
		}

		public FluentConfiguration OutputLogTo(TextWriter output)
		{
			new TextWriterTestResultAppender(runner, output);
			return this;
		}

		public FluentConfiguration WhenFinishedCall(EventHandler<SqlTestRunCompleteEventArgs> handler)
		{
			runner.SqlTestRunComplete += handler;
			return this;
		}

		public FluentConfiguration AfterEachTestCall(EventHandler<SqlTestCompleteEventArgs> handler)
		{
			runner.SqlTestComplete += handler;
			return this;
		}

		public void RunOn(IDbConnection con)
		{
			connection = con;
			Run();
		}

		public void Run()
		{
			if (runner == null)
				throw new InvalidOperationException("No ISqlTestRunner specified. Use FromFilesIn to specify the test load location");

			if (connection == null)
				throw new InvalidOperationException("No database connection specified. Use OnConnection or RunOn to provide one");

			runner.ExecuteAll(connection);
		}
	}
}
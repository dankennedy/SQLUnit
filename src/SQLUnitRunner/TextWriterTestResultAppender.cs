using System;
using System.Data;
using System.IO;

namespace SQLUnitRunner
{
	/// <summary>
	/// Simple helper class which provides some formatting output
	/// for test results.  Intended to output content to text log
	/// file or the console.
	/// </summary>
	public class TextWriterTestResultAppender
	{
		private readonly ISqlTestRunner testRunner;
		private readonly TextWriter output;

		/// <summary>
		/// Create a new instance and wire up the events for the 
		/// specified <see cref="ISqlTestRunner"/>
		/// </summary>
		/// <param name="runner">Loads and executes tests and raises relevant events</param>
		/// <param name="target">Acts as the destination to output logging content to</param>
		public TextWriterTestResultAppender(ISqlTestRunner runner, TextWriter target)
		{
			if (runner == null) throw new ArgumentNullException("runner", "Test runner required");
			if (target == null) throw new ArgumentNullException("target", "Output writer required");

			output = target;
			testRunner = runner;

			testRunner.SqlTestComplete += runner_SqlTestComplete;
			testRunner.SqlTestRunComplete += runner_SqlTestRunComplete;
		}

		void runner_SqlTestComplete(object sender, SqlTestCompleteEventArgs e)
		{
			output.WriteLine("Test: {0} ({1})\r\n{2}", e.Test.Name, e.Result.Passed ? "Passed" : "Failed", e.Result.Message);
			if (!e.Result.Passed && e.Result.ErrorTable != null)
			{
				foreach (DataColumn col in e.Result.ErrorTable.Columns)
					output.Write(col.ColumnName + "\t");

				output.Write("\r\n");
				foreach (DataRow row in e.Result.ErrorTable.Rows)
				{
					foreach (DataColumn col in e.Result.ErrorTable.Columns)
						output.Write((row[col] is DBNull ? "(null)" : row[col]) + "\t");

					output.Write("\r\n");
				}
			}
			output.WriteLine("-----------------------------------------------");
		}

		void runner_SqlTestRunComplete(object sender, SqlTestRunCompleteEventArgs e)
		{
			output.WriteLine();
			output.WriteLine("Summary");
			output.WriteLine("-----------------------------------------------");
			output.WriteLine("Passed: " + e.TestsPassed);
			output.WriteLine("Failed: " + e.TestsFailed);
			output.WriteLine("Time Taken: " + e.TimeTaken);
			output.WriteLine("-----------------------------------------------");

			testRunner.SqlTestComplete -= runner_SqlTestComplete;
			testRunner.SqlTestRunComplete -= runner_SqlTestRunComplete;
		}

	}
}
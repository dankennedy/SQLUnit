using System;
using System.Collections.Generic;
using System.Data;

namespace SQLUnitRunner
{
	/// <summary>
	/// The definition of a container that can load and run multiple tests
	/// </summary>
	public interface ISqlTestRunner
	{
		/// <summary>
		/// The object to use when loading tests
		/// </summary>
		ISqlTestLoader TestLoader { get; }

		/// <summary>
		/// Executes all loaded tests
		/// </summary>
		/// <param name="connection">The database connection against which to execute</param>
		/// <returns>A collection of results</returns>
		IList<ISqlTestResult> ExecuteAll(IDbConnection connection);

		/// <summary>
		/// Raised when a test has finished running either passed or failed
		/// </summary>
		event EventHandler<SqlTestCompleteEventArgs> SqlTestComplete;

		/// <summary>
		/// Raised at the end of a test run with summary results
		/// </summary>
		event EventHandler<SqlTestRunCompleteEventArgs> SqlTestRunComplete;
	}
}
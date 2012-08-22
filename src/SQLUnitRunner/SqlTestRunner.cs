using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SQLUnitRunner
{
	/// <summary>
	/// Base implementation of <see cref="ISqlTestRunner"/> which executes all
	/// tests returned by TestLoader, provides a central event source
	/// for the calling application and raises summary event info when
	/// complete
	/// </summary>
	public class SqlTestRunner : ISqlTestRunner
	{
		public event EventHandler<SqlTestCompleteEventArgs> SqlTestComplete;
		public event EventHandler<SqlTestRunCompleteEventArgs> SqlTestRunComplete;

		public SqlTestRunner(ISqlTestLoader loader)
		{
			TestLoader = loader;
		}

		/// <summary>
		/// Responsible for loading a set of tests to execute
		/// </summary>
		public ISqlTestLoader TestLoader { get; private set; }

		public IList<ISqlTestResult> ExecuteAll(IDbConnection connection)
		{
			var startTime = DateTime.Now;
			var results = new List<ISqlTestResult>();
			foreach (var testFixture in TestLoader.Fixtures)
			{
				var result = testFixture.Execute(connection);
				OnSqlTestComplete(testFixture, result);
				results.Add(result);
			}
			OnSqlTestRunComplete(results, DateTime.Now.Subtract(startTime));
			return results;
		}

		protected void OnSqlTestComplete(ISqlTestFixture sender, ISqlTestResult result)
		{
			if (SqlTestComplete != null)
				SqlTestComplete(this, new SqlTestCompleteEventArgs(sender, result));
		}

		protected void OnSqlTestRunComplete(IEnumerable<ISqlTestResult> results, TimeSpan timeTaken)
		{
			if (SqlTestRunComplete != null)
			{
				SqlTestRunComplete(this, new SqlTestRunCompleteEventArgs
					(
						results.Count(r => r.Passed),
						results.Count(r => !r.Passed),
						timeTaken
					));
			}
		}

	}
}
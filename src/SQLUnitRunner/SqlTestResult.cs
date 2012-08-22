using System.Data;

namespace SQLUnitRunner
{
	/// <summary>
	/// Basic implementation of <see cref="ISqlTestResult"/> which 
	/// encapsulates the results of executing a test
	/// </summary>
	public class SqlTestResult : ISqlTestResult
	{
		public SqlTestResult(bool passed) : this(passed, string.Empty, null) { }

		public SqlTestResult(bool passed, string message) : this(passed, message, null) { }

		public SqlTestResult(bool passed, string message, DataTable rows)
		{
			Passed = passed;
			ErrorTable = rows;
			Message = message;
		}

		public string Message { get; private set; }
		public bool Passed { get; private set; }
		public DataTable ErrorTable { get; private set; }
	}
}
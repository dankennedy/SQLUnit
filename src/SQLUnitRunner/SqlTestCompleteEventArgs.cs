using System;

namespace SQLUnitRunner
{
	/// <summary>
	/// Encapsulates the results of a <see cref="ISqlTestFixture"/> test run
	/// </summary>
	public class SqlTestCompleteEventArgs : EventArgs
	{
		public readonly ISqlTestResult Result;
		public readonly ISqlTestFixture Test;

		public SqlTestCompleteEventArgs(ISqlTestFixture test, ISqlTestResult result)
		{
			Result = result;
			Test = test;
		}
	}
}
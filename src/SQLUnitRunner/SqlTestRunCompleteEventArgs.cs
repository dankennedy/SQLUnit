using System;

namespace SQLUnitRunner
{
	/// <summary>
	/// Encapsulates the results of a completed test run
	/// where all tests in a collection/list have been
	/// executed and their results compiled
	/// </summary>
	public class SqlTestRunCompleteEventArgs : EventArgs
	{
		public readonly int TestsPassed;
		public readonly int TestsFailed;
		public readonly TimeSpan TimeTaken;

		public SqlTestRunCompleteEventArgs(int testsPassed, int testsFailed, TimeSpan timeTaken)
		{
			if (testsPassed < 0)
				throw new ArgumentOutOfRangeException("testsPassed", "Number of tests passed cannot be a negative number");

			if (testsPassed < 0)
				throw new ArgumentOutOfRangeException("testsFailed", "Number of tests failed cannot be a negative number");

			TestsPassed = testsPassed;
			TestsFailed = testsFailed;
			TimeTaken = timeTaken;
		}

		public int TestsTaken { get { return TestsPassed + TestsFailed; } }
	}
}
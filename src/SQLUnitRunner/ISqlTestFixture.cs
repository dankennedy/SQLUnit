using System;
using System.Data;

namespace SQLUnitRunner
{
	///<summary>
	///An individual test that can be executed
	///</summary>
	public interface ISqlTestFixture
	{	
		///<summary>
		///Descriptive name
		///</summary>
		string Name { get; }

		/// <summary>
		///The actual Sql that gets executed for the test
		/// </summary>
		string Sql { get; }

		///<summary>
		///Execute the test
		///</summary>
		///<returns>The <see cref="ISqlTestResult" /> result</returns>
		ISqlTestResult Execute(IDbConnection connection);

		/// <summary>
		/// Raised when a test has finished running either passed or failed
		/// </summary>
		event EventHandler<SqlTestCompleteEventArgs> SqlTestComplete;
	}
}
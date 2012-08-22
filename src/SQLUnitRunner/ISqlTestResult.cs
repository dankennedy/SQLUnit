using System.Data;

namespace SQLUnitRunner
{
	///<summary>
	///The result of running a <see cref="ISqlTestFixture"/>
	///</summary>
	public interface ISqlTestResult
	{
		///<summary>
		///Whether the test passed or not
		///</summary>
		bool Passed { get; }

		/// <summary>
		///A descriptive message, usually on why the test failed
		/// </summary>
		string Message { get; }

		///<summary>
		///Erroneous data rows produced by the test
		///</summary>
		DataTable ErrorTable { get; }
	}
}
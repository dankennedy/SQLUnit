using System.Collections.Generic;

namespace SQLUnitRunner
{
	///<summary>
	/// Responsible for providing a set of tests to run
	///</summary>
	public interface ISqlTestLoader
	{
		/// <summary>
		/// Provides the ability to read and create the tests to populate the Fixtures
		/// property
		/// </summary>
		ITestReader TestReader { get; }

		///<summary>
		/// A set of executable tests
		///</summary>
		IEnumerable<ISqlTestFixture> Fixtures { get; }
	}
}
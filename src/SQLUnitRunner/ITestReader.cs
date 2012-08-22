using System.IO;

namespace SQLUnitRunner
{
	/// <summary>
	/// Provides the ability to read the incoming stream and create a 
	/// <see cref="ISqlTestFixture"/>from it
	/// </summary>
	public interface ITestReader
	{
		/// <summary>
		/// Reads and parses the incoming stream and generates a concrete implementation of
		/// <see cref="ISqlTestFixture"/>
		/// </summary>
		/// <param name="input">Text based test definition</param>
		/// <returns>An <see cref="ISqlTestFixture"/> with properties set from the
		/// specified stream</returns>
		ISqlTestFixture Create(StreamReader input);
	}
}